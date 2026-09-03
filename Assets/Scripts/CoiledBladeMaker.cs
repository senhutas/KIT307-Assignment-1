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
        float swordHeight = 4.0f;

        // Extend the profile along the path for the length of the sword.
        Matrix4x4[] path = new Matrix4x4[2];
        path[0] = Matrix4x4.Translate(new Vector3(0, 0.0f, 0));
        path[1] = Matrix4x4.Translate(new Vector3(0, swordHeight, 0));

        meshFilter.mesh = MeshUtilities.Sweep(bladeProfile, path, false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}