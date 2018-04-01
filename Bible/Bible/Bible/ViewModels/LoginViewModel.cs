namespace Bible.ViewModels
{
    
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Services;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion


        #region Attributes

        private string password;
        private string email;
        private bool isRunning;
        private bool isEnabled;
        #endregion


        #region Properties
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsRemembered
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        #endregion

        #region Constructor

        public LoginViewModel()
        {
            this.dataService = new DataService();
            this.apiService = new ApiService();
            this.IsRemembered = true;
            this.IsEnabled = true;            
        }

        #endregion

        #region Commands
        public ICommand LoginInstagramCommand
        {
            get
            {
                return new RelayCommand(LoginInstagram);
            }
        }

        private async void LoginInstagram()
        {
            await Application.Current.MainPage.Navigation.PushAsync(
                    new LoginInstagramPage());
        }

        public ICommand LoginFacebookComand
        {
            get
            {
                return new RelayCommand(LoginFacebook);
            }
        }

        private async void LoginFacebook()
        {
            await Application.Current.MainPage.Navigation.PushAsync(
                new LoginFacebookPage());
        }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }

        }
        
        public ICommand SignUpCommand
        {
            get
            {
                return new RelayCommand(SignUp);
            }

        }

        private async void SignUp()
        {
            MainViewModel.GetInstance().SingUp = new SingUpViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new SingUpPage());
        }

        private async void Login()
        {

            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "There was an error!!",
                    "you must enter an Email...",
                    "Ok");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "There was an error!!",
                    "you must enter a password...",
                    "Ok");

                return;
            }
            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                                  "There was an error!!",
                                  "you must connect to internet... " ,
                                  "Ok");

                return;
            }

            var JWTservice = Application.Current.Resources["JSONwebToken"].ToString();
            var token = await this.apiService.GetToken(
                JWTservice,
                this.Email,
                this.Password);

            if (token == null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                                  "Error",
                                  "Something was wrong, please try again later...",
                                  "Ok");
                return;
            }

            if (string.IsNullOrEmpty(token.AccessToken))
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                                  "Error",
                                  token.ErrorDescription,
                                  "Ok");
                return;
            }

            var apiService = Application.Current.Resources["APISecurity"].ToString();
            var user = await this.apiService.GetUserByEmail(
                apiService,
                "/api",
                "/Users/GetUserByEmail",
                token.TokenType,
                token.AccessToken,
                this.Email);

            var userLocal = Helpers.Converter.ToUserLocal(user);

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token.AccessToken;
            mainViewModel.TokenType = token.TokenType;
            mainViewModel.User = user;

            if (this.IsRemembered)
            {
                Settings.Token = token.AccessToken;
                Settings.TokenType = token.TokenType;
                this.dataService.DeleteAllAndInsert(userLocal);
            }
            

            mainViewModel.Bibles = new BiblesViewModel();            
            Application.Current.MainPage = new MasterPage();

            this.IsRunning = false;
            this.IsEnabled = true;

            this.Email = string.Empty;
            this.Password = string.Empty;

        }
        #endregion
    }
}
