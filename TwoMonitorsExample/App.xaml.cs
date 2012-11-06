using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace TwoMonitorsExample
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			OpenBothWindows();
		}

		private void OpenBothWindows()
		{
			OpenWindowInPrimaryScreen();

			OpenWindowInSecondaryScreen();

			//OpenWindowInAllScreens();
		}


		public static void OpenWindowInPrimaryScreen()
		{
			var primaryScreen = Screen.AllScreens.FirstOrDefault(s => s.Primary);
			if (primaryScreen == null) return;
			var window = new MainWindow
				             {
					             WindowStartupLocation = WindowStartupLocation.Manual,
					             Top = 0,
					             Left = 0,
					             Width = primaryScreen.WorkingArea.Width,
					             Height = primaryScreen.WorkingArea.Height,
					             WindowState = WindowState.Maximized,
					             WindowStyle = WindowStyle.None,
					             ResizeMode = ResizeMode.NoResize
				             };

			window.Show();
			window.Focus();
		}

		public static void OpenWindowInSecondaryScreen()
		{
			var secondaryScreen = Screen.AllScreens.FirstOrDefault(s => !s.Primary);

			if (secondaryScreen != null)
			{
				var window = new FullscreenWindow();
				window.WindowStartupLocation = WindowStartupLocation.Manual;
				window.Top = secondaryScreen.WorkingArea.Top;
				window.Left = secondaryScreen.WorkingArea.Left;
				window.Width = secondaryScreen.WorkingArea.Width;
				window.Height = secondaryScreen.WorkingArea.Height;

				window.Show();
			}
		}

		public static void OpenWindowInAllScreens()
		{
			var window = new FullscreenWindow();
			window.labelContent.Content = "Window in all screens";

			window.WindowStartupLocation = WindowStartupLocation.Manual;
			double width = 0;
			double height = 0;
			var primaryScreen = Screen.AllScreens.FirstOrDefault(s => s.Primary);

			foreach (var screen in Screen.AllScreens)
			{
				width += screen.WorkingArea.Width;
				height += screen.WorkingArea.Height;
			}
			
			window.Top = 0;
			window.Left = 0;
			window.Width = primaryScreen.WorkingArea.Left + width;
			window.Height = primaryScreen.WorkingArea.Bottom + height;

			window.Show();
		}
	}
}
