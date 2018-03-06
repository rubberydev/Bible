namespace Bible.ViewModels
{
    using Models;

    public class MainViewModel
    {
        #region Properties
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

        public BibliesViewModel Biblies
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
