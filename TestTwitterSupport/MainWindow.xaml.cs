using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using Microsoft.Win32;
using TweetSharp;

namespace TestTwitterSupport
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private TwitterClientInfo _twitterClientInfo;
		private TwitterService _twitterService;
		private OAuthRequestToken _requestToken;
		private OAuthAccessToken _accessToken;

		private string _imagePath;
		private string _imageName;

		public MainWindow()
		{
			InitializeComponent();

			InitializeNewSession();
			GoToTwitterUrl();

			SymbolsCounter.Text = "0\nof\n140";
		}

		private void InitializeNewSession()
		{
			_twitterClientInfo = new TwitterClientInfo();
			_twitterClientInfo.ConsumerKey = "2yeyhTAMl3Euzc2aXSlOA"; //TODO: get this value especially for adidas
			_twitterClientInfo.ConsumerSecret = "8yXVBamNBfd82hSbCbBNWPQvto0cjgqVjYuBbLXm0";
				//TODO: get this value especially for adidas
			_twitterService = new TwitterService(_twitterClientInfo);
			_requestToken = _twitterService.GetRequestToken();
		}

		private void GoToTwitterUrl()
		{
			string authUrl = _twitterService.GetAuthorizationUri(_requestToken).ToString();
			BrowserControl.Navigate(authUrl);
		}

		private void OpenImage_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (openFileDialog.ShowDialog().Value)
			{
				_imagePath = openFileDialog.FileName;
				_imageName = Path.GetFileName(_imagePath);
				UploadedImage.Source = new BitmapImage(new Uri(_imagePath, UriKind.Absolute));
			}

			Tweet.IsEnabled = TweetText.Text.Length > 0 || !string.IsNullOrEmpty(_imagePath);
		}

		private void Tweet_Click(object sender, RoutedEventArgs e)
		{
			string pin = Code.Text;
			_accessToken = _twitterService.GetAccessToken(_requestToken, pin);

			//string token = _accessToken.Token; //Attach the Debugger and put a break point here
			//string tokenSecret = _accessToken.TokenSecret; //And another Breakpoint here

			UploadPhoto(_imagePath, _imageName, TweetText.Text);
		}

		private void TweetText_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (TweetText.Text.Length > 140)
			{
				TweetText.Text = TweetText.Text.Substring(0, 140);
			}

			SymbolsCounter.Text = string.Format("{0}\nof\n140", TweetText.Text.Length);

			Tweet.IsEnabled = TweetText.Text.Length > 0 || !string.IsNullOrEmpty(_imagePath);
		}

		private void UploadPhoto(string imagePath, string imageName, string message)
		{
			var credentials = new OAuthCredentials
			                  	{
			                  		Type = OAuthType.ProtectedResource,
			                  		SignatureMethod = OAuthSignatureMethod.HmacSha1,
			                  		ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
			                  		ConsumerKey = _twitterClientInfo.ConsumerKey,
			                  		ConsumerSecret = _twitterClientInfo.ConsumerSecret,
			                  		Token = _accessToken.Token,
			                  		TokenSecret = _accessToken.TokenSecret,
			                  		Version = "1.0a"
			                  	};


			RestClient restClient = new RestClient
			                        	{
			                        		Authority = "https://upload.twitter.com",
			                        		//HasElevatedPermissions = true,
			                        		Credentials = credentials,
			                        		Method = WebMethod.Post
			                        	};
			RestRequest restRequest = new RestRequest
			                          	{
			                          		Path = "1/statuses/update_with_media.json"
			                          	};

			restRequest.AddParameter("status", message);
			restRequest.AddFile("media[]", imageName, imagePath, "image/jpg");

			restClient.BeginRequest(restRequest, PostTweetRequestCallback);
		}


		private void PostTweetRequestCallback(RestRequest request, RestResponse response, object obj)
		{
			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				//Success code
			}
		}
	}
}
