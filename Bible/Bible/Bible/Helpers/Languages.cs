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

        public static string Books
        {
            get { return Resource.Books; }
        }

        public static string Bibles
        {
            get { return Resource.Bibles; }
        }

        public static string AdvancedSearch
        {
            get { return Resource.AdvancedSearch; }
        }

        public static string SearchByKeyword
        {
            get { return Resource.SearchByKeyword; }
        }

        public static string Verses
        {
            get { return Resource.Verses; }
        }

        public static string Chapter
        {
            get { return Resource.Chapter; }
        }

        public static string BeforeChapter
        {
            get { return Resource.BeforeChapter; }
        }

        public static string NextChapter
        {
            get { return Resource.NextChapter; }
        }

        public static string LoginFacebook
        {
            get { return Resource.LoginFacebook; }
        }

        public static string LoginInstagram
        {
            get { return Resource.LoginInstagram; }
        }

        public static string Menu
        {
            get { return Resource.Menu; }
        }

        public static string SearchByVerse
        {
            get { return Resource.SearchByVerse; }
        }

        public static string ResultSearch
        {
            get { return Resource.ResultSearch; }
        }

        public static string Book
        {
            get { return Resource.Book; }
        }

        public static string ChapterVerses
        {
            get { return Resource.ChapterVerses; }
        }

        public static string Example
        {
            get { return Resource.Example; }
        }

        public static string SearchButton
        {
            get { return Resource.SearchButton; }
        }

        public static string KeyWord
        {
            get { return Resource.KeyWord; }
        }

        public static string Example2
        {
            get { return Resource.Example2; }
        }

        public static string Names
        {
            get { return Resource.Names; }
        }

        public static string PlaceNames
        {
            get { return Resource.PlaceNames; }
        }

        public static string LastNames
        {
            get { return Resource.LastNames; }
        }

        public static string PlaceLastNam
        {
            get { return Resource.PlaceLastNam; }
        }

        public static string Telephone
        {
            get { return Resource.Telephone; }
        }

        public static string PlacePhone
        {
            get { return Resource.PlacePhone; }
        }

        public static string Confirm
        {
            get { return Resource.Confirm; }
        }

        public static string ConfirmPass
        {
            get { return Resource.ConfirmPass; }
        }

        public static string Register
        {
            get { return Resource.Register; }
        }

        public static string ChangeImage
        {
            get { return Resource.ChangeImage; }
        }

        public static string ValidationPassword
        {
            get { return Resource.ValidationPassword; }
        }

        public static string InternetValidation
        {
            get { return Resource.InternetValidation; }
        }

        public static string SomethingWrong
        {
            get { return Resource.SomethingWrong; }
        }

        public static string MenuHambSearch1
        {
            get { return Resource.MenuHambSearch1; }
        }

        public static string MenuHambSearch2
        {
            get { return Resource.MenuHambSearch2; }
        }

        public static string LogOut
        {
            get { return Resource.LogOut; }
        }

        public static string SelectBook
        {
            get { return Resource.SelectBook; }
        }

        public static string ValidFormat
        {
            get { return Resource.ValidFormat; }
        }

        public static string ValidVerses
        {
            get { return Resource.ValidVerses; }
        }

        public static string RequiredKeyword
        {
            get { return Resource.RequiredKeyword; }
        }

        public static string Congratulation
        {
            get { return Resource.Congratulation; }
        }

        public static string LocationImage
        {
            get { return Resource.LocationImage; }
        }

        public static string Cancel
        {
            get { return Resource.Cancel; }
        }

        public static string FromGallery
        {
            get { return Resource.FromGallery; }
        }

        public static string FromCamera
        {
            get { return Resource.FromCamera; }
        }

        public static string TypeNames
        {
            get { return Resource.TypeNames; }
        }

        public static string TypeLastNames
        {
            get { return Resource.TypeLastNames; }
        }

        public static string TypeEmail
        {
            get { return Resource.TypeEmail; }
        }

        public static string TypeEmailValid
        {
            get { return Resource.TypeEmailValid; }
        }

        public static string TypePhoneNumber
        {
            get { return Resource.TypePhoneNumber; }
        }

        public static string SixCharacterPass
        {
            get { return Resource.SixCharacterPass; }
        }

        public static string TypeConfirmPass
        {
            get { return Resource.TypeConfirmPass; }
        }

        public static string KeysMatch
        {
            get { return Resource.KeysMatch; }
        }

        public static string SuccessRegister
        {
            get { return Resource.SuccessRegister; }
        }
               
    }

}
