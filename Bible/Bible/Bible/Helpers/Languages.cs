namespace Bible.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    using Resources;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string EmailValidation
        {
            get { return Resource.EmailValidation; }
        }

        public static string Rememberme
        {
            get { return Resource.Rememberme; }
        }

        public static string EmailPlace
        {
            get { return Resource.EmailPlace; }
        }
        
        public static string BibleTitle
        {
            get { return Resource.BibleTitle; }
        }

        public static string Email
        {
            get { return Resource.Email; }
        }

        public static string Password
        {
            get { return Resource.Password; }
        }

        public static string PlacePass
        {
            get { return Resource.PlacePass; }
        }

        public static string ForgotPass
        {
            get { return Resource.ForgotPass; }
        }

        public static string Login
        {
            get { return Resource.Login; }
        }

        public static string SignUp
        {
            get { return Resource.SignUp; }
        }

    }

}
