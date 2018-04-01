[assembly: Xamarin.Forms.ExportRenderer(
    typeof(Bible.Views.LoginInstagramPage),
    typeof(Bible.iOS.Implementations.LoginInstagramPageRenderer))]

namespace Bible.iOS.Implementations
{
    using Models;
    using Services;
    using System;
    using System.Threading.Tasks;
    using Xamarin.Auth;
    using Xamarin.Forms.Platform.iOS;

    public class LoginInstagramPageRenderer : PageRenderer
    {
        bool done = false;

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (done)
            {
                return;
            }

            var InstagramAppID = Xamarin.Forms.Application.Current.Resources["InstagramAppID"].ToString();
            var InstagramAuthURL = Xamarin.Forms.Application.Current.Resources["InstagramAuthURL"].ToString();
            var InstagramRedirectURL = Xamarin.Forms.Application.Current.Resources["InstagramRedirectURL"].ToString();
            var InstagramScope = Xamarin.Forms.Application.Current.Resources["InstagramScope"].ToString();

            var auth = new OAuth2Authenticator(
                clientId: InstagramAppID,
                scope: InstagramScope,
                authorizeUrl: new Uri(InstagramAuthURL),
                redirectUrl: new Uri(InstagramRedirectURL));

            auth.Completed += async (sender, eventArgs) =>
            {
                DismissViewController(true, null);
                App.HideLoginView();

                if (eventArgs.IsAuthenticated)
                {
                    var accessToken = eventArgs.Account.Properties["access_token"].ToString();
                    var profile = await GetInstagramProfileAsync(accessToken);
                    App.NavigateToProfile(profile, "Instagram");
                }
                else
                {
                    App.HideLoginView();
                }
            };
            done = true;
            PresentViewController(auth.GetUI(), true, null);
        }

        public async Task<InstagramResponse> GetInstagramProfileAsync(string accessToken)
        {
            var InstagramProfileInfoURL = Xamarin.Forms.Application.Current.Resources["InstagramProfileInfoURL"].ToString();
            var requestUrl = string.Format("{0}={1}",
                InstagramProfileInfoURL,
                accessToken);

            var apiService = new ApiService();
            return await apiService.GetInstagram(requestUrl);
        }
    }
}
