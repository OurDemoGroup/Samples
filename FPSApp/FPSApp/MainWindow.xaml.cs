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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using FPSApp.GeometricPrimitives;

namespace FPSApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Scene.SceneViewModel scene;
        public MainWindow()
        {
            InitializeComponent();
            scene = new Scene.SceneViewModel();
            model.Content = scene.ModelGroup;
        }

        private void SleepButton_Click(object sender, RoutedEventArgs e)
        {
            int milliseconds = 0;
            try
            {
                milliseconds = Convert.ToInt32(SleepValueTextBox.Text);
            }
            catch (System.FormatException ex)
            {
                MessageBox.Show("Only digits are valid. \n" + ex.Message);
            }
            
            Thread.Sleep(milliseconds);
        }

        private void CrashButton_Click(object sender, RoutedEventArgs e)
        {
            int milliseconds = Convert.ToInt32(SleepValueTextBox.Text);
            int zero = 0;
            milliseconds /= zero;
        }

        private void DummyButtin_Click(object sender, RoutedEventArgs e)
        {
            for (Int16 i = Int16.MinValue; i < Int16.MaxValue; i++)
            {
            }
        }

        private void AddMeshButton_Click(object sender, RoutedEventArgs e)
        {
            scene.AddSphereMesh(5, 0);
        }
    }
}
