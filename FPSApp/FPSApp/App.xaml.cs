using System;
using System.Configuration;
using System.Data;
using System.Windows;

using System.ServiceModel;
using System.ServiceModel.Channels;

namespace FPSApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private HealthServiceClient healthService;
        private WindowsController windowCreator;

        public App()
        {
            windowCreator = new WindowsController(new MainWindow(), false);
            windowCreator.AddRelativeWindow(new SecondaryWindow());
            healthService = new HealthServiceClient();
        }
    }
}
