using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Timers;
using System.Windows.Threading;

namespace InactiveUserApplication
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private DispatcherTimer activityTimer; 
		public MainWindow()
		{
			InitializeComponent();

			InputManager.Current.PreProcessInput += Activity;

			activityTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(10),
				IsEnabled = true
			};
			activityTimer.Tick += Inactivity;
		}

		void Inactivity(object sender, EventArgs e)
		{
			rectangle1.Visibility = Visibility.Hidden; // Update
			// Console.WriteLine("INACTIVE " + DateTime.Now.Ticks);
		}

		void Activity(object sender, PreProcessInputEventArgs e)
		{
			rectangle1.Visibility = Visibility.Visible; // Update
			// Console.WriteLine("ACTIVE " + DateTime.Now.Ticks);

			activityTimer.Stop();
			activityTimer.Start();
		}
	}
}
