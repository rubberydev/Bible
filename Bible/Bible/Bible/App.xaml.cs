namespace Bible
{
    using Models;
    using Helpers;
    using Services;
    using ViewModels;
    using Views;
    using Xamarin.Forms;
    using System.Threading.Tasks;
    using System;

    public partial class App : Application
	{
        #region Properties
        public static NavigationPage Navigator
        {
            get;
            internal set;
        }

        public static MasterPage Master
        {
            get;
            internal set;
        }
        #endregion


        #region Constructor
        public App()
        {
            InitializeComponent();

            if(string.IsNullOrEmpty(Settings.Token))
            {
                this.MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                var dataService = new DataService();
                var user = dataService.First<User>(false);
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = Settings.Token;
                mainViewModel.TokenType = Settings.TokenType;
                mainViewModel.User = user;
                mainViewModel.Bibles = new BiblesViewModel();
                this.MainPage = new MasterPage();
            }

            if (Settings.IsRemembered == "true")
            {
                var dataService = new DataService();
                var token = dataService.First<TokenResponse>(false);

                if (token != null && token.Expires > DateTime.Now)
                {
                    var user = dataService.First<User>(false);
                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Token_ = token;
                    mainViewModel.User = user;
                    mainViewModel.Bibles = new BiblesViewModel();
                    Application.Current.MainPage = new MasterPage();
                }
                else
                {
                    this.MainPage = new NavigationPage(new LoginPage());
                }
            }          
        }

        public static Action HideLoginView_
        {
            get
            {
                return new Action(() => App.Current.MainPage = 
                                  new NavigationPage(new LoginPage()));
            }
        }
        public static async Task NavigateToProfile<T>(T profile, string socialNetwork)
        {
            //switch (socialNetwork)
            //{
            //    case "Instagram":
                    //En este objeto tenemos todos los datos del usuario
                    InstagramResponse ResponseSocialNetwork = profile as InstagramResponse;  
                    
                    if(ResponseSocialNetwork == null)
                    {
                        Application.Current.MainPage = new NavigationPage(new LoginPage());
                        return;
                    }
                    //=====================================================================================
                    var apiService = new ApiService();
                    var dataService = new DataService();

                    var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
                    var token = await apiService.LoginInstagram(
                        apiSecurity,
                        "/api",
                        "/Users/LoginInstagram",
                        ResponseSocialNetwork);            

                    if (token == null)
                    {
                        Application.Current.MainPage = new NavigationPage(new LoginPage());
                        return;
                    }

                    var user = await apiService.GetUserByEmail(
                        apiSecurity,
                        "/api",
                        "/Users/GetUserByEmail",
                        token.TokenType,
                        token.AccessToken,
                        token.UserName);

                    User userLocal = null;
                    if (user != null)
                    {
                        userLocal = Converter.ToUserLocal(user);
                        dataService.DeleteAllAndInsert(userLocal);
                        dataService.DeleteAllAndInsert(token);
                    }

                    var mainViewModel = MainViewModel.GetInstance();
                    mainViewModel.Token_ = token;
                    mainViewModel.User = userLocal;
                    Settings.IsRemembered = "true";

                    mainViewModel.Bibles = new BiblesViewModel();
                    Application.Current.MainPage = new MasterPage();                  
                         
        }

        #endregion

        #region Methods
        public static Action HideLoginView
        {
            get
            {
                return new Action(() => Application.Current.MainPage =
                                  new NavigationPage(new LoginPage()));
            }
        }

        public static async Task NavigateToProfile(FacebookResponse profile)
        {
            if (profile == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }

            var apiService = new ApiService();
            var dataService = new DataService();

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var token = await apiService.LoginFacebook(
                apiSecurity,
                "/api",
                "/Users/LoginFacebook",
                profile);

            if (token == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }

            var user = await apiService.GetUserByEmail(
                apiSecurity,
                "/api",
                "/Users/GetUserByEmail",
                token.TokenType,
                token.AccessToken,
                token.UserName);

            User userLocal = null;
            if (user != null)
            {
                userLocal = Converter.ToUserLocal(user);
                dataService.DeleteAllAndInsert(userLocal);
                dataService.DeleteAllAndInsert(token);
            }

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token_ = token;
            mainViewModel.User = userLocal;            
            Settings.IsRemembered = "true";

            mainViewModel.Bibles = new BiblesViewModel();
            Application.Current.MainPage = new MasterPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        } 
        #endregion
    }
}
