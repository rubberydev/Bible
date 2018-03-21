namespace Bible.ViewModels
{
    using Models;
    using System;
    using System.Collections.ObjectModel;

    public class MainViewModel
    {
        #region ViewModels
        public string Token
        {
            get;
            set;
        }

        public LoginViewModel Login
        {
            get;
            set;
        }

        public BiblesViewModel Bibles
        {
            get;
            set;
        }

        public BibleViewModel Bible
        {
            get;
            set;
        }

        public BookViewModel Book
        {
            get;
            set;
        }

        public string TokenType
        {
            get;
            set;
        }
        #endregion

        #region Properties
        public string SelectedModule
        {
            get;
            set;
        }

        public ObservableCollection<MenuItemViewModel> Menus
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
            this.LoadMenu();
        }
        
        #endregion

        #region Singleton

        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
        #endregion

        #region Methods
        private void LoadMenu()
        {
            this.Menus = new ObservableCollection<MenuItemViewModel>();            

            this.Menus.Add(new MenuItemViewModel
            {
                Icon = "ic_search",
                Title = "Advanced search",
                PageName = "BiblesSearchPage"
            });

            this.Menus.Add(new MenuItemViewModel
            {
                Icon = "ic_search",
                Title = "Search by key word",
                PageName = "BiblesSearchPage"
            });

            this.Menus.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                Title = "Log out",
                PageName = "LoginPage"
            });
        }
        #endregion

    }
}
