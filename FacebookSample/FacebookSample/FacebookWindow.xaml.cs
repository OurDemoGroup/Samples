using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Facebook;

namespace FacebookSample
{
	/// <summary>
	/// Interaction logic for FacebookWindow.xaml
	/// </summary>
	public partial class FacebookWindow : Window
	{
		private const string AppId = "298556803577343";
		private readonly FacebookClient _facebookClient = new FacebookClient();
		/// <summary>
		/// Extended permissions is a comma separated list of permissions to ask the user.
		/// </summary>
		/// <remarks>
		/// For extensive list of available extended permissions refer to 
		/// https://developers.facebook.com/docs/reference/api/permissions/
		/// </remarks>
		private const string ExtendedPermissions = "user_about_me,user_photos,photo_upload,read_stream,publish_stream";

		public FacebookWindow()
		{
			InitializeComponent();

			webBrowser.LoadCompleted += WebBrowserOnLoadCompleted;

			var loginUrl = GetFacebookLoginUrl(AppId, ExtendedPermissions);
			if (loginUrl != null)
				webBrowser.Navigate(loginUrl);
		}

		private void WebBrowserOnLoadCompleted(object sender, NavigationEventArgs navigationEventArgs)
		{
			FacebookOAuthResult oauthResult;
			if (!_facebookClient.TryParseOAuthCallbackUrl(navigationEventArgs.Uri, out oauthResult))
			{
				return;
			}

			if (oauthResult.IsSuccess)
			{
				try
				{
					byte[] imageBytes = new byte[0];
					using (MemoryStream ms = new MemoryStream())
					{
						Resource.va55qx0t.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
						imageBytes = ms.ToArray();
					}

					_facebookClient.AccessToken = oauthResult.AccessToken;

					_facebookClient.PostCompleted += (o, e) =>
					{
						if (e.Cancelled || e.Error != null)
						{
							return;
						}

						var result = e.GetResultData();
					};

					var parameters = new Dictionary<string, object>();
					parameters["message"] = "my first photo upload using Facebook C# SDK";
					parameters["file"] = new FacebookMediaObject
					{
						ContentType = "image/jpeg",
						FileName = "image.jpeg"
					}.SetValue(imageBytes);

					_facebookClient.PostTaskAsync("me/photos", parameters);
				}
				catch (FacebookApiException ex)
				{
					// handel error message 
				}
			}
		}

		private Uri GetFacebookLoginUrl(string appId, string extendedPermissions)
		{
			dynamic parameters = new ExpandoObject();
			parameters.client_id = appId;
			parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";
			parameters.response_type = "token";
			parameters.display = "popup";

			// add the 'scope' parameter only if we have extendedPermissions.
			if (!string.IsNullOrWhiteSpace(extendedPermissions))
			{
				// A comma-delimited list of permissions
				parameters.scope = extendedPermissions;
			}

			return _facebookClient.GetLoginUrl(parameters);
		}
	}
}
