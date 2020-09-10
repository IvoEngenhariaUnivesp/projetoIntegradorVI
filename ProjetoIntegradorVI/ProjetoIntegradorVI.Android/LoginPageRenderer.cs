using System;
using Xamarin.Forms;
using FBLoginTeste;
using Xamarin.Forms.Platform.Android;
using Android.App;
using Xamarin.Auth;
using Newtonsoft.Json.Linq;
using ProjetoIntegradorVI.View;
using ProjetoIntegradorVI;

[assembly: ExportRenderer (typeof (FBCadastro), typeof (FBLoginTeste.Droid.LoginPageRenderer))]

namespace FBLoginTeste.Droid
{
	public class LoginPageRenderer : PageRenderer
	{

        public LoginPageRenderer()
		{
			var activity = this.Context as Activity;

			var auth = new OAuth2Authenticator (
				clientId: "2034617650012167", // your OAuth2 client id
				scope: "email", // the scopes for the particular API you're accessing, delimited by "+" symbols
				authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
				redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

			auth.Completed += async (sender, eventArgs) => {
				if (eventArgs.IsAuthenticated) {
					var accessToken = eventArgs.Account.Properties ["access_token"].ToString ();
					var expiresIn = Convert.ToDouble (eventArgs.Account.Properties ["expires_in"]);
					var expiryDate = DateTime.Now + TimeSpan.FromSeconds (expiresIn);

					var request = new OAuth2Request ("GET", new Uri ("https://graph.facebook.com/me?fields=email,name,id"), null, eventArgs.Account);
					var response = await request.GetResponseAsync ();
					var obj = JObject.Parse (response.GetResponseText ());

					var id = obj ["id"].ToString ().Replace ("\"", ""); 
					var name = obj ["name"].ToString ().Replace ("\"", "");
					var email = obj["email"].ToString().Replace("\"", "");
					App.Current.MainPage.Navigation.PushModalAsync(new Login(true,name, id,email));
				} else {
					App.Current.MainPage.Navigation.PushModalAsync(new Login(false, "", "",""));
				}
			};

			activity.StartActivity (auth.GetUI(activity));	
		}
	}
}