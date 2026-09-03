using UnityEngine;

public class MeshUtilities
{
    // A function to sweep a profile along a path.
    public static Mesh Sweep(Vector3[] profile, Matrix4x4[] path, bool closed)
    {
        Mesh mesh = new Mesh();

        int numVerts = path.Length * profile.Length;
        int numTris;

        if (closed)
            numTris = 2 * path.Length * profile.Length;
        else
            numTris = 2 * (path.Length - 1) * profile.Length;


        Vector3[] vertices = new Vector3[numVerts];
        int[] tris = new int[numTris * 3];

        for (int i = 0; i < path.Length; i++)
        {
            for (int j = 0; j < profile.Length; j++)
            {
                Vector3 v = path[i].MultiplyPoint(profile[j]);
                vertices[i * profile.Length + j] = v;

                if (closed || i < path.Length - 1)
                {

                    tris[6 * (i * profile.Length + j)] = (j + i * profile.Length);
                    tris[6 * (i * profile.Length + j) + 1] = ((j + 1) % profile.Length + i * profile.Length);
                    tris[6 * (i * profile.Length + j) + 2] = ((j + 1) % profile.Length + ((i + 1) % path.Length) * profile.Length);
                    tris[6 * (i * profile.Length + j) + 3] = (j + i * profile.Length);
                    tris[6 * (i * profile.Length + j) + 4] = ((j + 1) % profile.Length + ((i + 1) % path.Length) * profile.Length);
                    tris[6 * (i * profile.Length + j) + 5] = (j + ((i + 1) % path.Length) * profile.Length);
                }
            }
        }

        mesh.vertices = vertices;

        mesh.triangles = tris;

        mesh.RecalculateNormals();

        return mesh;
    }
}
