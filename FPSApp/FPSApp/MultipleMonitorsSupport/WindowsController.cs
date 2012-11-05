using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FPSApp
{
    public class WindowsController
    {
        #region Fields

        public Window PrimaryWindow { get { return primaryWindow; } }
        public List<Window> RelativeWindows { get { return relativeWindows; } }
        private Window primaryWindow;
        private List<Window> relativeWindows;
        private bool primaryWindowHoldAllSpace;

        #endregion

        #region Public Methods

        public WindowsController(Window window, bool isPrimaryWindowHoldAllSpace)
        {
            primaryWindowHoldAllSpace = isPrimaryWindowHoldAllSpace;
            relativeWindows = new List<Window>();
            primaryWindow = window;
            SetupNewWindow(ref primaryWindow);
            primaryWindow.Show();

            primaryWindow.Left = 0;
            primaryWindow.Top = 0;
            if (primaryWindowHoldAllSpace)
            {
                primaryWindow.Width = SystemParameters.VirtualScreenWidth;
                primaryWindow.Height = SystemParameters.VirtualScreenHeight;
            }
        }

        public void AddRelativeWindow(Window window)
        {
            if (primaryWindow == null)
            {
                return;
            }
            SetupNewWindow(ref window);
            RelativeWindows.Add(window);
            window.Show();
        }

        #endregion

        #region Window Customizung

        private void SetupNewWindow(ref Window window)
        {
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.WindowStyle = WindowStyle.None;
            window.ResizeMode = ResizeMode.NoResize;
            //window.Topmost = true;
            window.Left = CurrentWindowsWidth();
            window.Loaded += windowLoaded;
        }

        private void windowLoaded(object sender, RoutedEventArgs e)
        {
            Window window = (Window)sender;
            if (window == PrimaryWindow && primaryWindowHoldAllSpace)
            {
                return;
            }
            window.WindowState = WindowState.Maximized;
        }

        // because all added windows are "Maximazed" we can just get
        // theirs width, without computing smth. like
        // width = SystemParameters.PrimaryScreenWidth * numScreens
        // and convert to DPI * (width / 96)
        private double CurrentWindowsWidth()
        {
            double currentWindowsWidth = PrimaryWindow.Width;
            foreach (Window window in relativeWindows)
            {
                currentWindowsWidth += window.Width;
            }
            return currentWindowsWidth;
        }

        #endregion
    }
}
