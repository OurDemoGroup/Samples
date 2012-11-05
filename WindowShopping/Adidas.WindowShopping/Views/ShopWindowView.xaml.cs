using System.Windows;

namespace Adidas.WindowShopping.Views
{
	public partial class ShopWindowView
	{
		public ShopWindowView()
		{
			InitializeComponent();
			
		}

		private void ShopWindowLoaded(object sender, RoutedEventArgs e)
		{
			var window = Parent as Window;
			if (window == null)
				return;

			window.ResizeMode = ResizeMode.NoResize;
		}
	}
}
