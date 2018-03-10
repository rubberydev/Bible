namespace Bible.ViewModels
{
    using Models;

    public class MainViewModel
    {
        #region ViewModels
        public TokenResponse Token
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
        #endregion

        #region Properties
        public string SelectedModule
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


    }
}
