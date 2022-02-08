﻿using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace BasicLoader.Interface
{
    /// <summary>
    /// Representation of the smallest part of a model. The part consists of a name, a list of vertices and triangle indices.
    /// </summary>
    public interface IPart
    {

        /// <summary>
        /// List of vertices for the mesh of a part.
        /// </summary>
        IList<Vector3D> Vertices { get; }
        /// <summary>
        /// A list of indices which describes the triangles of the mesh.
        /// </summary>
        IList<int> Triangles { get; }
        /// <summary>
        /// A displayable name of the part.
        /// </summary>
        string Name { get; }

       // Matrix3x3 Rotation { get; set; }
        Vector3D Position { get; set; }
    }
}