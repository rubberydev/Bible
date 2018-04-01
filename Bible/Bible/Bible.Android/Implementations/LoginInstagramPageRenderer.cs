[assembly: Xamarin.Forms.ExportRenderer(
    typeof(Bible.Views.LoginInstagramPage),
    typeof(Bible.Droid.Implementations.LoginInstagramPageRenderer))]

namespace Bible.Droid.Implementations
{
    using Android.App;
    using Bible.Services;
    using Models;
    using System;
    using System.Threading.Tasks;
    using Xamarin.Auth;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    public class LoginInstagramPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            var activity = this.Context as Activity;

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
                if (eventArgs.IsAuthenticated)
                {
                    var accessToken = eventArgs.Account.Properties["access_token"].ToString();
                    var profile = await GetInstagramProfileAsync(accessToken);
                    App.NavigateToProfile(profile, "Instagram");
                }
                else
                {
                    App.HideLoginView_();
                }
            };

            activity.StartActivity(auth.GetUI(activity));
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
