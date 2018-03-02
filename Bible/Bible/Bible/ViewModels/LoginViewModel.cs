namespace Bible.ViewModels
{
    using Views;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {

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
           
            this.IsRemembered = true;
            this.IsEnabled = true;

            this.Email = "juliohg8913@gmail.com";
            this.Password = "Developer123.";
        }

        #endregion

        #region Commands

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }

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

            if(Email != "juliohg8913@gmail.com" || Password != "Developer123.")
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "There was an error!!",
                    "Incorrect Keys...",
                    "Ok");

                return;

            }

            var mainViewModel = MainViewModel.GetInstance();
            
            mainViewModel.Biblies = new BibliesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new BibliesPage());

            this.IsRunning = false;
            this.IsEnabled = true;

            this.Email = string.Empty;
            this.Password = string.Empty;

        }
        #endregion
    }
}
