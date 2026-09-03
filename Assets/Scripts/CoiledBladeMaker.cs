using UnityEngine;

public class CoiledBladeMaker : MonoBehaviour
{
    public Material bladeMaterial;  // The double-sided material for the blade.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = bladeMaterial;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        // Create a profile with diamond caps with an open inner point than join together (the blade profile).
        Vector3[] bladeProfile = new Vector3[]
        {
            // Top centre point.
            new Vector3(0.00f, 0.0f, 0.008f),
            
            // Right diamond cap.
            new Vector3(0.08f, 0.0f, 0.020f),
            new Vector3(0.12f, 0.0f, 0.000f),
            new Vector3(0.12f, 0.0f, 0.000f),
            new Vector3(0.08f, 0.0f, -0.020f),
            
            // Bottom centre point.
            new Vector3(0.00f, 0.0f, -0.008f),
            
            // Left diamond cap.
            new Vector3(-0.08f, 0.0f, -0.020f),
            new Vector3(-0.12f, 0.0f, 0.000f),
            new Vector3(-0.12f, 0.0f, 0.000f),
            new Vector3(-0.08f, 0.0f, 0.020f)
        };

        // Sword values.
        float bladeHeight = 4.0f;
        int divisions = 100;
        float numTurns = 2.0f;      // Number of rotations of the blade 360 degrees along the length of the blade.

        // Extend the profile along the path for the length of the blade.
        Matrix4x4[] path = new Matrix4x4[divisions + 1];

        // Modified MakeCirclePath function from MeshUtilities to "coil" or rotate the blade along the path.
        for (int i = 0; i <= divisions; i++)
        {
            float angle = (360.0f * i * numTurns) / divisions;
            float yIncrement = (bladeHeight * i) / divisions;   // AI helped fix this value. Calcualtes the y value for each step of the sword.

            path[i] = Matrix4x4.Rotate(Quaternion.Euler(0, -angle, 0)) * Matrix4x4.Translate(new Vector3(0, yIncrement, 0));
        }

        meshFilter.mesh = MeshUtilities.Sweep(bladeProfile, path, false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}