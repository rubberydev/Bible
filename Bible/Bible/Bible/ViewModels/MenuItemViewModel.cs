namespace Bible.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class MenuItemViewModel
    {
        #region Properties
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }

        }
        #endregion

        #region Methods
        private void Navigate()
        {
            App.Master.IsPresented = false;

            if (this.PageName == "LoginPage")
            {
                Settings.Token = string.Empty;
                Settings.TokenType = string.Empty;
                Settings.IsRemembered = "false";
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = string.Empty;
                mainViewModel.TokenType = string.Empty;
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            else if (this.PageName == "BiblesSearchPage")
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Bibles = new BiblesViewModel();
                App.Navigator.PushAsync(new BiblesSearchPage());  
                
            }
            else if (this.PageName == "BiblesSearchWordPage")
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Bibles = new BiblesViewModel();
                App.Navigator.PushAsync(new BiblesSearchWordPage());

            }
            else if (this.PageName == "MyProfilePage")
            {
                MainViewModel.GetInstance().MyProfile = new MyProfileViewModel();
                App.Navigator.PushAsync(new MyProfilePage());
            }
        }
        #endregion
    }
}
