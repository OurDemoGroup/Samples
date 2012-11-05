using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FPSApp
{
    /// <summary>
    /// Interaction logic for SecondaryWindow.xaml
    /// </summary>
    public partial class SecondaryWindow : Window
    {
        private Scene.SceneViewModel scene;
        public SecondaryWindow()
        {
            InitializeComponent();
            scene = new Scene.SceneViewModel();
            model.Content = scene.ModelGroup;
        }

        private void SleepButton_Click(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(5000);
        }

        private void AddMeshButton_Click(object sender, RoutedEventArgs e)
        {
            scene.AddSphereMesh(5, 0);
            scene.AddCubeMesh(5, 5);
        }
    }
}
