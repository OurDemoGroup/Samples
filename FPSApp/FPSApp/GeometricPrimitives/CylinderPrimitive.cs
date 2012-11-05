using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace FPSApp.GeometricPrimitives
{
    public class CylinderPrimitive : GeometricPrimitive
    {
        /// <summary>
        /// Constructs a new cylinder primitive,
        /// with the specified size and tessellation level.
        /// </summary>
        public CylinderPrimitive(Material material,
                                 float height, float diameter, int tessellation)
        {
            if (tessellation < 3)
                throw new ArgumentOutOfRangeException("tessellation");

            height /= 2;
            float radius = diameter / 2;
            Vector3D down = new Vector3D(0, -1, 0);
            Vector3D up = new Vector3D(0, 1, 0);

            // Create a ring of triangles around the outside of the cylinder.
            for (int i = 0; i < tessellation; i++)
            {
                Vector3D normal = GetCircleVector(i, tessellation);
                Vector3D vertex = normal * radius + up * height;
                Point3D vertexPoint = new Point3D(vertex.X, vertex.Y, vertex.Z);
                AddVertex(vertexPoint, normal);

                vertex = normal * radius + down * height;
                vertexPoint = new Point3D(vertex.X, vertex.Y, vertex.Z);
                AddVertex(vertexPoint, normal);

                AddIndex(i * 2);
                AddIndex(i * 2 + 1);
                AddIndex((i * 2 + 2) % (tessellation * 2));

                AddIndex(i * 2 + 1);
                AddIndex((i * 2 + 3) % (tessellation * 2));
                AddIndex((i * 2 + 2) % (tessellation * 2));
            }

            // Create flat triangle fan caps to seal the top and bottom.
            CreateCap(tessellation, height, radius, up);
            CreateCap(tessellation, height, radius, down);

            InitializePrimitive(material);
        }


        /// <summary>
        /// Helper method creates a triangle fan to close the ends of the cylinder.
        /// </summary>
        void CreateCap(int tessellation, float height, float radius, Vector3D normal)
        {
            // Create cap indices.
            for (int i = 0; i < tessellation - 2; i++)
            {
                if (normal.Y > 0)
                {
                    AddIndex(CurrentVertex);
                    AddIndex(CurrentVertex + (i + 1) % tessellation);
                    AddIndex(CurrentVertex + (i + 2) % tessellation);
                }
                else
                {
                    AddIndex(CurrentVertex);
                    AddIndex(CurrentVertex + (i + 2) % tessellation);
                    AddIndex(CurrentVertex + (i + 1) % tessellation);
                }
            }

            // Create cap vertices.
            for (int i = 0; i < tessellation; i++)
            {
                Vector3D vertex = GetCircleVector(i, tessellation) * radius +
                                   normal * height;
                Point3D vertexPoint = new Point3D(vertex.X, vertex.Y, vertex.Z);
                AddVertex(vertexPoint, normal);
            }
        }


        /// <summary>
        /// Helper method computes a point on a circle.
        /// </summary>
        static Vector3D GetCircleVector(int i, int tessellation)
        {
            float angle = i * ((float)Math.PI * 2.0f) / tessellation;

            float dx = (float)Math.Cos(angle);
            float dz = (float)Math.Sin(angle);

            return new Vector3D(dx, 0, dz);
        }
    }
}
