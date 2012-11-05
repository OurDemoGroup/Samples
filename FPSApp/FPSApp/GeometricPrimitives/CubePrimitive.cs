using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace FPSApp.GeometricPrimitives
{
    public class CubePrimitive : GeometricPrimitive
    {
        /// <summary>
        /// Constructs a new cube primitive, with the specified size.
        /// </summary>
        public CubePrimitive(Material material, float size)
        {
            // A cube has six faces, each one pointing in a different direction.
            Vector3D[] normals =
            {
                new Vector3D(0, 0, 1),
                new Vector3D(0, 0, -1),
                new Vector3D(1, 0, 0),
                new Vector3D(-1, 0, 0),
                new Vector3D(0, 1, 0),
                new Vector3D(0, -1, 0),
            };

            // Create each face in turn.
            foreach (Vector3D normal in normals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                Vector3D side1 = new Vector3D(normal.Y, normal.Z, normal.X);
                Vector3D side2 = Vector3D.CrossProduct(normal, side1);

                // Six indices (two triangles) per face.
                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 1);
                AddIndex(CurrentVertex + 2);

                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 2);
                AddIndex(CurrentVertex + 3);

                // Four vertices per face.
                Vector3D vertex = (normal - side1 - side2) * size / 2;
                Point3D vertexPoint = new Point3D(vertex.X, vertex.Y, vertex.Z);
                AddVertex(vertexPoint, normal);
                AddTextureCoordinates(new System.Windows.Point(0,0));

                vertex = (normal - side1 + side2) * size / 2;
                vertexPoint = new Point3D(vertex.X, vertex.Y, vertex.Z);
                AddVertex(vertexPoint, normal);
                AddTextureCoordinates(new System.Windows.Point(0, 1));

                vertex = (normal + side1 + side2) * size / 2;
                vertexPoint = new Point3D(vertex.X, vertex.Y, vertex.Z);
                AddVertex(vertexPoint, normal);
                AddTextureCoordinates(new System.Windows.Point(1, 0));

                vertex = (normal + side1 - side2) * size / 2;
                vertexPoint = new Point3D(vertex.X, vertex.Y, vertex.Z);
                AddVertex(vertexPoint, normal);
                AddTextureCoordinates(new System.Windows.Point(1, 1));
            }

            InitializePrimitive(material);
        }
    }
}
