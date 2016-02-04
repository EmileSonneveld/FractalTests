using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.Reflection;
using UnityEditor.SceneManagement;

public class GeometryMaker
{

    public static Mesh MakePrism()
    {

        var prism = new Mesh();
        const int count = 3;
        var verts = new Vector3[count * 2];
        var indices = new List<int>(3 * count + 6); // inc caps
        var normals = new List<Vector3>(count * 2);

        for (int i = 0; i < count; i++)
        {
            float step = Mathf.PI * 2 / count;
            verts[i * 2 + 0] = new Vector3(Mathf.Cos(step * i), -0.5f, Mathf.Sin(step * i));
            verts[i * 2 + 1] = new Vector3(Mathf.Cos(step * i), +0.5f, Mathf.Sin(step * i));
            normals.Add(verts[i * 2 + 0].normalized);
            normals.Add(verts[i * 2 + 1].normalized);

            indices.Add((i * 2 + 2) % (count * 2));
            indices.Add((i * 2 + 1) % (count * 2));
            indices.Add((i * 2 + 0) % (count * 2));
            indices.Add((i * 2 + 1) % (count * 2));
            indices.Add((i * 2 + 2) % (count * 2));
            indices.Add((i * 2 + 3) % (count * 2));
        }

        // The cap depends on the fat that is is a triangular prism.
        indices.AddRange(new int[] { 1, 3, 5 });
        indices.AddRange(new int[] { 4, 2, 0 });
        indices.Reverse();

        prism.vertices = verts;
        prism.SetNormals(normals);
        prism.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);

        return prism;
    }
}
