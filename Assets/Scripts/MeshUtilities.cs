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

    public static Mesh WallWithHole(Vector3 wallSize, Vector2 holePosition, float holeRadius, int holeDivisions)
    {
        Mesh mesh = new Mesh();

        // Translates the holePosition to have its 0, 0, 0 value be the bottom left corner of the cube.
        float halfWallX = wallSize.x * 0.5f;
        float halfWallY = wallSize.y * 0.5f;
        float halfWallZ = wallSize.z * 0.5f;

        Vector2 holeCenter = new Vector2(holePosition.x - halfWallX, holePosition.y - halfWallY);

        Vector3[] vertices = new Vector3[4 + 4 + 16 + (holeDivisions * 4)];     // Array of cube and cylinder verticies + duplicate vertices for each face of the cube and
                                                                                // edges of the cylinder (sharp lighting).

        // Vertices for front and back corners of the cube.
        vertices[0] = new Vector3(-halfWallX, -halfWallY, halfWallZ);
        vertices[1] = new Vector3(halfWallX, -halfWallY, halfWallZ);
        vertices[2] = new Vector3(halfWallX, halfWallY, halfWallZ);
        vertices[3] = new Vector3(-halfWallX, halfWallY, halfWallZ);
        
        vertices[4] = new Vector3(-halfWallX, -halfWallY, -halfWallZ);
        vertices[5] = new Vector3(halfWallX, -halfWallY, -halfWallZ);
        vertices[6] = new Vector3(halfWallX, halfWallY, -halfWallZ);
        vertices[7] = new Vector3(-halfWallX, halfWallY, -halfWallZ);

        // The position in the vertices array that associated vertices start at.
        int frontHoleStart = 8;
        int backHoleStart = 8 + holeDivisions;
        int insideFrontStart = 8 + holeDivisions * 2;
        int insideBackStart = 8 + holeDivisions * 3;

        // From Cylinder function of MeshUtilities in tutorial work (slightly edited).
        float dTheta = Mathf.PI * 2.0f / holeDivisions;
        for (int i = 0; i < holeDivisions; i++)
        {
            float theta = i * dTheta;
            float x = holeCenter.x + holeRadius * Mathf.Cos(theta);
            float y = holeCenter.y + holeRadius * Mathf.Sin(theta);
            // Front rim of hole.
            vertices[frontHoleStart + i] = new Vector3(x, y, halfWallZ);
            // Back rim of hole.
            vertices[backHoleStart + i] = new Vector3(x, y, -halfWallZ);
            // Duplicate for inside front rim of hole.
            vertices[insideFrontStart + i] = new Vector3(x, y, halfWallZ);
            // Duplicate for inside back rim of hole.
            vertices[insideBackStart + i] = new Vector3(x, y, -halfWallZ);
        }

        int sidesFacesStart = 8 + holeDivisions * 4;    // Starting vertice for the duplicate vertices on the side faces of the cube.

        // AI helped me get the ordering of this section correct as some of my faces were inside out. 
        // Duplicate vertices of cube corners for sharp lighting.
        // Top face of cube.
        vertices[sidesFacesStart] = new Vector3(-halfWallX, halfWallY, halfWallZ);
        vertices[sidesFacesStart + 1] = new Vector3(halfWallX, halfWallY, halfWallZ);
        vertices[sidesFacesStart + 2] = new Vector3(halfWallX, halfWallY, -halfWallZ);
        vertices[sidesFacesStart + 3] = new Vector3(-halfWallX, halfWallY, -halfWallZ);
        // Bottom face of cube.
        vertices[sidesFacesStart + 4] = new Vector3(-halfWallX, -halfWallY, halfWallZ);
        vertices[sidesFacesStart + 5] = new Vector3(-halfWallX, -halfWallY, -halfWallZ);
        vertices[sidesFacesStart + 6] = new Vector3(halfWallX, -halfWallY, -halfWallZ);
        vertices[sidesFacesStart + 7] = new Vector3(halfWallX, -halfWallY, halfWallZ);
        // Left face of cube.
        vertices[sidesFacesStart + 8] = new Vector3(-halfWallX, halfWallY, halfWallZ);
        vertices[sidesFacesStart + 9] = new Vector3(-halfWallX, halfWallY, -halfWallZ);
        vertices[sidesFacesStart + 10] = new Vector3(-halfWallX, -halfWallY, -halfWallZ);
        vertices[sidesFacesStart + 11] = new Vector3(-halfWallX, -halfWallY, halfWallZ);
        // Right face of cube.
        vertices[sidesFacesStart + 12] = new Vector3(halfWallX, halfWallY, halfWallZ);
        vertices[sidesFacesStart + 13] = new Vector3(halfWallX, -halfWallY, halfWallZ);
        vertices[sidesFacesStart + 14] = new Vector3(halfWallX, -halfWallY, -halfWallZ);
        vertices[sidesFacesStart + 15] = new Vector3(halfWallX, halfWallY, -halfWallZ);

        mesh.vertices = vertices;

        int[] tris = new int[(holeDivisions * 3 + 12) * 2 + 24 + (holeDivisions * 6)];  // The number of vertex references required to create all the triangles.
        int currentTris = 0;                                                            // The current index.

        int quarterDivisions = holeDivisions / 4;        // The number of divisions in each quarter of the hole.

        // Creates the triangles of the square face (Modified logic from the draw cap triangles for-loop in Cylinder() of MeshUtilities).
        for (int i = 0; i < holeDivisions; i++)
        {
            tris[currentTris++] = (i / quarterDivisions + 2) % 4;
            tris[currentTris++] = frontHoleStart + (i + 1) % holeDivisions;
            tris[currentTris++] = frontHoleStart + i;

            // AI did help me error check this section, I got the gist of it, but couldn't quite get the formulas.
            // If the next vertex is in the next quadrant (0, 90, 180, 270 degrees), also create connection to next square corner.
            if ((i / quarterDivisions + 2) % 4 != ((i + 1) % holeDivisions / quarterDivisions + 2) % 4)
            {
                tris[currentTris++] = (i / quarterDivisions + 2) % 4;
                tris[currentTris++] = ((i + 1) % holeDivisions / quarterDivisions + 2) % 4;
                tris[currentTris++] = frontHoleStart + (i + 1) % holeDivisions;
            }
        }

        // Creates the triangles of the back face (Modified logic from the draw cap triangles for-loop in Cylinder() of MeshUtilities).
        for (int i = 0; i < holeDivisions; i++)
        {
            tris[currentTris++] = 4 + (i / quarterDivisions + 2) % 4;
            tris[currentTris++] = backHoleStart + i;
            tris[currentTris++] = backHoleStart + (i + 1) % holeDivisions;

            // AI did help me error check this section, I got the gist of it, but couldn't quite get the formulas.
            // If the next vertex is in the next quadrant (0, 90, 180, 270 degrees), also create connection to next square corner.
            if ((i / quarterDivisions + 2) % 4 != ((i + 1) % holeDivisions / quarterDivisions + 2) % 4)
            {
                tris[currentTris++] = 4 + (i / quarterDivisions + 2) % 4;
                tris[currentTris++] = backHoleStart + (i + 1) % holeDivisions;
                tris[currentTris++] = 4 + ((i + 1) % holeDivisions / quarterDivisions + 2) % 4;
            }
        }

        // Creates the faces of the left, right, bottom, and top walls.
        for (int i = 0; i < 4; i++)
        {
            // First triangle of the wall face.
            tris[currentTris++] = sidesFacesStart + i * 4;
            tris[currentTris++] = sidesFacesStart + i * 4 + 1;
            tris[currentTris++] = sidesFacesStart + i * 4 + 2;

            // Second triangle of wall face.
            tris[currentTris++] = sidesFacesStart + i * 4;
            tris[currentTris++] = sidesFacesStart + i * 4 + 2;
            tris[currentTris++] = sidesFacesStart + i * 4 + 3;
        }

        // Creates faces of the inside of the hole in the wall.
        for (int i = 0; i < holeDivisions; i++)
        {
            // First triangle of the hole face.
            tris[currentTris++] = insideFrontStart + i;
            tris[currentTris++] = insideBackStart + (i + 1) % holeDivisions;
            tris[currentTris++] = insideBackStart + i;

            // second triangle of the hole face.
            tris[currentTris++] = insideFrontStart + i;
            tris[currentTris++] = insideFrontStart + (i + 1) % holeDivisions;
            tris[currentTris++] = insideBackStart + (i + 1) % holeDivisions;
        }

        mesh.triangles = tris;

        mesh.RecalculateNormals();

        return mesh;
    }

    public static Mesh WallWithTwoHoles(Vector3 wallSize, Vector2 hole1Position, float hole1Radius, Vector2 hole2Position, float hole2Radius, int holeDivisions)
    {
        Mesh mesh = new Mesh();

        // Translates the holePosition to have its 0, 0, 0 value be the bottom left corner of the cube.
        float halfWallX = wallSize.x * 0.5f;
        float halfWallY = wallSize.y * 0.5f;
        float halfWallZ = wallSize.z * 0.5f;

        Vector2 firstHoleCenter = new Vector2(hole1Position.x - halfWallX, hole1Position.y - halfWallY);
        Vector2 secondHoleCenter = new Vector2(hole2Position.x - halfWallX, hole2Position.y - halfWallY);

        Vector3[] vertices = new Vector3[4 + 4 + 16 + (holeDivisions * 8)];     // Array of cube and cylinder verticies + duplicate vertices for each face of the cube and
                                                                                // edges of the cylinder (sharp lighting).

        // Vertices for front and back corners of the cube.
        vertices[0] = new Vector3(-halfWallX, -halfWallY, halfWallZ);
        vertices[1] = new Vector3(halfWallX, -halfWallY, halfWallZ);
        vertices[2] = new Vector3(halfWallX, halfWallY, halfWallZ);
        vertices[3] = new Vector3(-halfWallX, halfWallY, halfWallZ);

        vertices[4] = new Vector3(-halfWallX, -halfWallY, -halfWallZ);
        vertices[5] = new Vector3(halfWallX, -halfWallY, -halfWallZ);
        vertices[6] = new Vector3(halfWallX, halfWallY, -halfWallZ);
        vertices[7] = new Vector3(-halfWallX, halfWallY, -halfWallZ);

        // The position in the vertices array that associated vertices start at for hole 1.
        int firstHoleFrontStart = 8;
        int firstHoleBackStart = 8 + holeDivisions;
        int firstHoleInsideFrontStart = 8 + holeDivisions * 2;
        int firstHoleInsideBackStart = 8 + holeDivisions * 3;

        // AI helped me double check these values as I made it WAY more complicated than it had to be, and kept running into errors on the inside faces, looking at it now, it really
        // shouldn't have been that complicated.
        // The position in the vertices array that associated vertices start at for hole 2.
        int secondHoleFrontStart = 8 + holeDivisions * 4;
        int secondHoleBackStart = 8 + holeDivisions * 5;
        int secondHoleInsideFrontStart = 8 + holeDivisions * 6;
        int secondHoleInsideBackStart = 8 + holeDivisions * 7;

        // From Cylinder function of MeshUtilities in tutorial work (slightly edited).
        float dTheta = Mathf.PI * 2.0f / holeDivisions;
        for (int i = 0; i < holeDivisions; i++)
        {
            float theta = i * dTheta;
            // Hole 1 vertices.
            float x1 = firstHoleCenter.x + hole1Radius * Mathf.Cos(theta);
            float y1 = firstHoleCenter.y + hole1Radius * Mathf.Sin(theta);
            // Front rim of hole.
            vertices[firstHoleFrontStart + i] = new Vector3(x1, y1, halfWallZ);
            // Back rim of hole.
            vertices[firstHoleBackStart + i] = new Vector3(x1, y1, -halfWallZ);
            // Duplicate for inside front rim of hole.
            vertices[firstHoleInsideFrontStart + i] = new Vector3(x1, y1, halfWallZ);
            // Duplicate for inside back rim of hole.
            vertices[firstHoleInsideBackStart + i] = new Vector3(x1, y1, -halfWallZ);

            // Hole 2 vertices.
            float x2 = secondHoleCenter.x + hole2Radius * Mathf.Cos(theta);
            float y2 = secondHoleCenter.y + hole2Radius * Mathf.Sin(theta);
            // Front rim of hole.
            vertices[secondHoleFrontStart + i] = new Vector3(x2, y2, halfWallZ);
            // Back rim of hole.
            vertices[secondHoleBackStart + i] = new Vector3(x2, y2, -halfWallZ);
            // Duplicate for inside front rim of hole.
            vertices[secondHoleInsideFrontStart + i] = new Vector3(x2, y2, halfWallZ);
            // Duplicate for inside back rim of hole.
            vertices[secondHoleInsideBackStart + i] = new Vector3(x2, y2, -halfWallZ);
        }

        int sidesFacesStart = 8 + holeDivisions * 8;    // Starting vertice for the duplicate vertices on the side faces of the cube.

        // AI helped me get the ordering of this section correct as some of my faces were inside out. 
        // Duplicate vertices of cube corners for sharp lighting.
        // Top face of cube.
        vertices[sidesFacesStart] = new Vector3(-halfWallX, halfWallY, halfWallZ);
        vertices[sidesFacesStart + 1] = new Vector3(halfWallX, halfWallY, halfWallZ);
        vertices[sidesFacesStart + 2] = new Vector3(halfWallX, halfWallY, -halfWallZ);
        vertices[sidesFacesStart + 3] = new Vector3(-halfWallX, halfWallY, -halfWallZ);
        // Bottom face of cube.
        vertices[sidesFacesStart + 4] = new Vector3(-halfWallX, -halfWallY, halfWallZ);
        vertices[sidesFacesStart + 5] = new Vector3(-halfWallX, -halfWallY, -halfWallZ);
        vertices[sidesFacesStart + 6] = new Vector3(halfWallX, -halfWallY, -halfWallZ);
        vertices[sidesFacesStart + 7] = new Vector3(halfWallX, -halfWallY, halfWallZ);
        // Left face of cube.
        vertices[sidesFacesStart + 8] = new Vector3(-halfWallX, halfWallY, halfWallZ);
        vertices[sidesFacesStart + 9] = new Vector3(-halfWallX, halfWallY, -halfWallZ);
        vertices[sidesFacesStart + 10] = new Vector3(-halfWallX, -halfWallY, -halfWallZ);
        vertices[sidesFacesStart + 11] = new Vector3(-halfWallX, -halfWallY, halfWallZ);
        // Right face of cube.
        vertices[sidesFacesStart + 12] = new Vector3(halfWallX, halfWallY, halfWallZ);
        vertices[sidesFacesStart + 13] = new Vector3(halfWallX, -halfWallY, halfWallZ);
        vertices[sidesFacesStart + 14] = new Vector3(halfWallX, -halfWallY, -halfWallZ);
        vertices[sidesFacesStart + 15] = new Vector3(halfWallX, halfWallY, -halfWallZ);

        mesh.vertices = vertices;

        int[] tris = new int[((holeDivisions * 3 + 12) * 2 + 24 + (holeDivisions * 6)) * 2];    // The number of vertex references required to create all the triangles.
        int currentTris = 0;                                                                    // The current index.

        int quarterDivisions = holeDivisions / 4;        // The number of divisions in each quarter of the hole.

        // Creates the triangles of the square face (Modified logic from the draw cap triangles for-loop in Cylinder() of MeshUtilities) for hole 1.
        for (int i = 0; i < holeDivisions; i++)
        {
            tris[currentTris++] = (i / quarterDivisions + 2) % 4;
            tris[currentTris++] = firstHoleFrontStart + (i + 1) % holeDivisions;
            tris[currentTris++] = firstHoleFrontStart + i;

            // AI did help me error check this section, I got the gist of it, but couldn't quite get the formulas.
            // If the next vertex is in the next quadrant (0, 90, 180, 270 degrees), also create connection to next square corner.
            if ((i / quarterDivisions + 2) % 4 != ((i + 1) % holeDivisions / quarterDivisions + 2) % 4)
            {
                tris[currentTris++] = (i / quarterDivisions + 2) % 4;
                tris[currentTris++] = ((i + 1) % holeDivisions / quarterDivisions + 2) % 4;
                tris[currentTris++] = firstHoleFrontStart + (i + 1) % holeDivisions;
            }
        }

        // Creates the triangles of the square face (Modified logic from the draw cap triangles for-loop in Cylinder() of MeshUtilities) for hole 2.
        for (int i = 0; i < holeDivisions; i++)
        {
            tris[currentTris++] = (i / quarterDivisions + 2) % 4;
            tris[currentTris++] = secondHoleFrontStart + (i + 1) % holeDivisions;
            tris[currentTris++] = secondHoleFrontStart + i;

            // AI did help me error check this section, I got the gist of it, but couldn't quite get the formulas.
            // If the next vertex is in the next quadrant (0, 90, 180, 270 degrees), also create connection to next square corner.
            if ((i / quarterDivisions + 2) % 4 != ((i + 1) % holeDivisions / quarterDivisions + 2) % 4)
            {
                tris[currentTris++] = (i / quarterDivisions + 2) % 4;
                tris[currentTris++] = ((i + 1) % holeDivisions / quarterDivisions + 2) % 4;
                tris[currentTris++] = secondHoleFrontStart + (i + 1) % holeDivisions;
            }
        }

        // Creates the triangles of the back face (Modified logic from the draw cap triangles for-loop in Cylinder() of MeshUtilities) for hole 1.
        for (int i = 0; i < holeDivisions; i++)
        {
            tris[currentTris++] = 4 + (i / quarterDivisions + 2) % 4;
            tris[currentTris++] = firstHoleBackStart + i;
            tris[currentTris++] = firstHoleBackStart + (i + 1) % holeDivisions;

            // AI did help me error check this section, I got the gist of it, but couldn't quite get the formulas.
            // If the next vertex is in the next quadrant (0, 90, 180, 270 degrees), also create connection to next square corner.
            if ((i / quarterDivisions + 2) % 4 != ((i + 1) % holeDivisions / quarterDivisions + 2) % 4)
            {
                tris[currentTris++] = 4 + (i / quarterDivisions + 2) % 4;
                tris[currentTris++] = firstHoleBackStart + (i + 1) % holeDivisions;
                tris[currentTris++] = 4 + ((i + 1) % holeDivisions / quarterDivisions + 2) % 4;
            }
        }

        // Creates the triangles of the back face (Modified logic from the draw cap triangles for-loop in Cylinder() of MeshUtilities) for hole 2.
        for (int i = 0; i < holeDivisions; i++)
        {
            tris[currentTris++] = 4 + (i / quarterDivisions + 2) % 4;
            tris[currentTris++] = secondHoleBackStart + i;
            tris[currentTris++] = secondHoleBackStart + (i + 1) % holeDivisions;

            // AI did help me error check this section, I got the gist of it, but couldn't quite get the formulas.
            // If the next vertex is in the next quadrant (0, 90, 180, 270 degrees), also create connection to next square corner.
            if ((i / quarterDivisions + 2) % 4 != ((i + 1) % holeDivisions / quarterDivisions + 2) % 4)
            {
                tris[currentTris++] = 4 + (i / quarterDivisions + 2) % 4;
                tris[currentTris++] = secondHoleBackStart + (i + 1) % holeDivisions;
                tris[currentTris++] = 4 + ((i + 1) % holeDivisions / quarterDivisions + 2) % 4;
            }
        }

        // Creates the faces of the left, right, bottom, and top walls.
        for (int i = 0; i < 4; i++)
        {
            // First triangle of the wall face.
            tris[currentTris++] = sidesFacesStart + i * 4;
            tris[currentTris++] = sidesFacesStart + i * 4 + 1;
            tris[currentTris++] = sidesFacesStart + i * 4 + 2;

            // Second triangle of wall face.
            tris[currentTris++] = sidesFacesStart + i * 4;
            tris[currentTris++] = sidesFacesStart + i * 4 + 2;
            tris[currentTris++] = sidesFacesStart + i * 4 + 3;
        }

        // Creates faces of the inside of the hole in the wall for hole 1.
        for (int i = 0; i < holeDivisions; i++)
        {
            // First triangle of the hole face.
            tris[currentTris++] = firstHoleInsideFrontStart + i;
            tris[currentTris++] = firstHoleInsideBackStart + (i + 1) % holeDivisions;
            tris[currentTris++] = firstHoleInsideBackStart + i;

            // second triangle of the hole face.
            tris[currentTris++] = firstHoleInsideFrontStart + i;
            tris[currentTris++] = firstHoleInsideFrontStart + (i + 1) % holeDivisions;
            tris[currentTris++] = firstHoleInsideBackStart + (i + 1) % holeDivisions;
        }

        // Creates faces of the inside of the hole in the wall for hole 2.
        for (int i = 0; i < holeDivisions; i++)
        {
            // First triangle of the hole face.
            tris[currentTris++] = secondHoleInsideFrontStart + i;
            tris[currentTris++] = secondHoleInsideBackStart + (i + 1) % holeDivisions;
            tris[currentTris++] = secondHoleInsideBackStart + i;

            // second triangle of the hole face.
            tris[currentTris++] = secondHoleInsideFrontStart + i;
            tris[currentTris++] = secondHoleInsideFrontStart + (i + 1) % holeDivisions;
            tris[currentTris++] = secondHoleInsideBackStart + (i + 1) % holeDivisions;
        }

        mesh.triangles = tris;

        mesh.RecalculateNormals();

        return mesh;
    }
}