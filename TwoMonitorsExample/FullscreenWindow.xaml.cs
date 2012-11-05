using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TwoMonitorsExample
{
	/// <summary>
	/// Interaction logic for FullscreenWindow.xaml
	/// </summary>
	public partial class FullscreenWindow : Window
	{
		public FullscreenWindow()
		{
			InitializeComponent();
		}

		private void Close_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
