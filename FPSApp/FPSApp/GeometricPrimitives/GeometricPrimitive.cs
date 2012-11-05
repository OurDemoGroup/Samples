using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace FPSApp.GeometricPrimitives
{
    /// <summary>
    /// Base class for simple geometric primitive models. This provides a vertex
    /// buffer, an index buffer, plus methods for drawing the model. Classes for
    /// specific types of primitive (CubePrimitive, SpherePrimitive, etc.) are
    /// derived from this common base, and use the AddVertex and AddIndex methods
    /// to specify their geometry.
    /// </summary>
    public abstract class GeometricPrimitive
    {
        #region Fields

        // During the process of constructing a primitive model, vertex
        // and index data is stored on the CPU in these managed lists.
        private MeshGeometry3D mesh;
        private GeometryModel3D geometricPrimitive;
        public GeometryModel3D GeometryModel { get { return geometricPrimitive; } }

        #endregion

        public GeometricPrimitive()
        {
            mesh = new MeshGeometry3D();
        }

        protected int CurrentVertex
        {
            get { return mesh.Positions.Count; }
        }

        protected void AddVertex(Point3D position, Vector3D normal)
        {
            mesh.Positions.Add(position);
            mesh.Normals.Add(normal);
        }

        protected void AddTextureCoordinates(Point texCoord)
        {
            mesh.TextureCoordinates.Add(texCoord);
        }

        protected void AddIndex(int index)
        {
            if (index > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            mesh.TriangleIndices.Add(index);
        }

        protected void InitializePrimitive(Material material)
        {
            geometricPrimitive = new GeometryModel3D(mesh, material);
        }
    }
}
