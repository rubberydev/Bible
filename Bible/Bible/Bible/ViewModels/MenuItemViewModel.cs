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
            if (this.PageName == "LoginPage")
            {
                Settings.Token = string.Empty;
                Settings.TokenType = string.Empty;
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = string.Empty;
                mainViewModel.TokenType = string.Empty;
                Application.Current.MainPage = new LoginPage();
            }

            if (this.PageName == "BiblesSearchPage")
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Bibles = new BiblesViewModel();
                App.Navigator.PushAsync(new BiblesSearchPage());                
            }

            if (this.PageName == "BiblesSearchWordPage")
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Bibles = new BiblesViewModel();
                App.Navigator.PushAsync(new BiblesSearchWordPage());
            }
        }
        #endregion
    }
}
