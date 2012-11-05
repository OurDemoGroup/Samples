using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using FPSApp.GeometricPrimitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FPSApp.Scene
{
    public class SceneViewModel
    {
        private Model3DGroup scene;
        private AmbientLight ambientLight;
        private DirectionalLight directionalLight;

        public Model3DGroup ModelGroup { get { return scene; } }

        public SceneViewModel()
        {
            scene = new Model3DGroup();

            ambientLight = new AmbientLight(Colors.DarkGray);
            directionalLight = new DirectionalLight(Colors.White, new Vector3D(-5, -5, -7));
            
            scene.Children.Add(ambientLight);
            scene.Children.Add(directionalLight);
        }

        public void AddSphereMesh(float sideSize, float yOffset)
        {
            float step = sideSize;
            for (int x = 0; x < sideSize; x++)
            {
                for (int y = 0; y < sideSize; y++)
                {
                    SpherePrimitive sphere = new SpherePrimitive(new DiffuseMaterial(Brushes.Red), 2.5f, 40);
                    sphere.GeometryModel.Transform = new TranslateTransform3D(step * (sideSize - x) - ((sideSize * sideSize) / 2.0f),
                        yOffset, 
                        step * (sideSize - y) - ((sideSize * sideSize) / 2.0f));
                    scene.Children.Add(sphere.GeometryModel);
                }
            }
        }

        public void AddCubeMesh(float sideSize, float yOffset)
        {
            var ib = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(@"Assets\footbalFiledDiffuse.jpg", UriKind.Relative))
            };

            DiffuseMaterial textureMaterial = new DiffuseMaterial(ib);

            float step = sideSize;
            for (int x = 0; x < sideSize; x++)
            {
                for (int y = 0; y < sideSize; y++)
                {
                    CubePrimitive cube = new CubePrimitive(textureMaterial, 2.5f);
                    cube.GeometryModel.Transform = new TranslateTransform3D(step * (sideSize - x) - ((sideSize * sideSize) / 2.0f),
                        yOffset,
                        step * (sideSize - y) - ((sideSize * sideSize) / 2.0f));
                    scene.Children.Add(cube.GeometryModel);
                }
            }
        }
    }
}
