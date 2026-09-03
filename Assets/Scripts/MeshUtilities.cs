using UnityEngine;

public class MeshUtilities
{
    // A function to sweep a profile along a path (from tutorial work).
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

    // Creates a cylinder.
    // divisions = number of divisions in the circle (there are no divisions in the height) (from tutorial work).
    public static Mesh Cylinder(int divisions, float radius, float height)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[divisions * 4 + 2]; // need top and bottom vertices
        float dTheta = Mathf.PI * 2.0f / divisions;
        for (int i = 0; i < divisions; i++)
        {
            float theta = i * dTheta;
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            // top vertex
            vertices[i] = new Vector4(x, height, z);
            // bottom vertex
            vertices[i + divisions] = new Vector4(x, -height, z);

            // Top cap rim vertex.
            vertices[i + divisions * 2] = new Vector4(x, height, z);
            // Bottom cap rim vertex.
            vertices[i + divisions * 3] = new Vector4(x, -height, z);
        }

        // Top center vertex.
        vertices[divisions * 4] = new Vector4(0, height, 0);
        // Bottom center vertex.
        vertices[divisions * 4 + 1] = new Vector4(0, -height, 0);

        mesh.vertices = vertices;

        int[] tris = new int[divisions * (6 + 3 + 3)];              // two tris for each side
        for (int i = 0; i < divisions; i++)
        {
            tris[i * 6] = i;                                        // current top vertex
            tris[i * 6 + 1] = (i + 1) % divisions;                  // next top vertex (wrapping)
            tris[i * 6 + 2] = divisions + (i + 1) % divisions;      // next bottom vertex (wrapping)

            tris[i * 6 + 3] = i;                                    // current top vertex
            tris[i * 6 + 4] = divisions + (i + 1) % divisions;      // next bottom vertex (wrapping)
            tris[i * 6 + 5] = divisions + i;                        // current bottom vertex
        }

        // Offsets for top and bottom caps for the triangle array.
        int topCapOffset = divisions * 6;
        int bottomCapOffset = divisions * 6 + divisions * 3;

        // Draw cap triangles.
        for (int i = 0; i < divisions; i++)
        {
            // Draw top cap.
            tris[topCapOffset + i * 3] = divisions * 4;
            tris[topCapOffset + i * 3 + 1] = divisions * 2 + (i + 1) % divisions; ;
            tris[topCapOffset + i * 3 + 2] = divisions * 2 + i;

            // Draw bottom cap.
            tris[bottomCapOffset + i * 3] = divisions * 4 + 1;
            tris[bottomCapOffset + i * 3 + 1] = divisions * 3 + i;
            tris[bottomCapOffset + i * 3 + 2] = divisions * 3 + (i + 1) % divisions; ;
        }

        mesh.triangles = tris;

        mesh.RecalculateNormals();

        return mesh;
    }
}
