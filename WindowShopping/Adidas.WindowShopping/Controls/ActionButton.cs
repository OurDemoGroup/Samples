using System.Windows;
using System.Windows.Controls;

namespace Adidas.WindowShopping.Controls
{
	public class ActionButton: Button
	{
		public static readonly DependencyProperty MovieIdProperty =
			DependencyProperty.Register("MovieId", typeof(string), typeof(ActionButton), new PropertyMetadata(null));

		public string MovieId
		{
			get { return (string)GetValue(MovieIdProperty); }
			set { SetValue(MovieIdProperty, value); }
		}
	}
}
