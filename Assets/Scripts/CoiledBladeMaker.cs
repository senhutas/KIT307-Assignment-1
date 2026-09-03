using System;
using UnityEngine;
using UnityEngine.UIElements;

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
            new Vector3(0.00f, 0.0f, 0.015f),
            
            // Right diamond cap.
            new Vector3(0.04f, 0.0f, 0.030f),
            new Vector3(0.04f, 0.0f, 0.030f),
            new Vector3(0.06f, 0.0f, 0.000f),
            new Vector3(0.06f, 0.0f, 0.000f),
            new Vector3(0.04f, 0.0f, -0.030f),
            new Vector3(0.04f, 0.0f, -0.030f),
            
            // Bottom centre point.
            new Vector3(0.00f, 0.0f, -0.015f),
            
            // Left diamond cap.
            new Vector3(-0.04f, 0.0f, -0.030f),
            new Vector3(-0.04f, 0.0f, -0.030f),
            new Vector3(-0.06f, 0.0f, 0.000f),
            new Vector3(-0.06f, 0.0f, 0.000f),
            new Vector3(-0.04f, 0.0f, 0.030f),
            new Vector3(-0.04f, 0.0f, 0.030f)
        };

        // Blade values.
        float bladeHeight = 1.5f;           // Height of blade.
        int divisions = 100;                // Number of divisions of blade.
        float numTurns = 2.0f;              // Number of rotations of the blade 360 degrees along the length of the blade.
        float tipStartPercentage = 0.90f;   // Where the main body of the blade ends, before the tip.

        // Extend the profile along the path for the length of the blade.
        Matrix4x4[] path = new Matrix4x4[divisions + 1];

        // Modified MakeCirclePath function from MeshUtilities to "coil" or rotate the blade along the path.
        for (int i = 0; i <= divisions; i++)
        {
            float angle = (360.0f * i * numTurns) / divisions;
            float yIncrement = (bladeHeight * i) / divisions;   // AI helped fix this value. Calcualtes the y value for each step of the sword, which I
                                                                // later used the logic of for the percentage of blade claculation.

            float scaleFactor;
            // Scale the body of blade down to 50% from start.
            if (((float)i / divisions) <= tipStartPercentage)
            {
                scaleFactor = 1.0f - ((((float)i / divisions) / tipStartPercentage) * 0.5f);
            }
            // Drastic scale at end of body to make tip not so needle like.
            else
            {
                scaleFactor = 0.5f * (1.0f - ((((float)i / divisions) - tipStartPercentage) / 0.10f)); // AI hinted at the problem (used the socratic method you mentioned),
                                                                                                       // which was I forgot to remove the tipStartPercentage.
            }

            path[i] = Matrix4x4.Translate(new Vector3(0, yIncrement, 0)) * Matrix4x4.Rotate(Quaternion.Euler(0, -angle, 0)) * Matrix4x4.Scale(new Vector3(scaleFactor, 1.0f, scaleFactor));
        }

        meshFilter.mesh = MeshUtilities.Sweep(bladeProfile, path, false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}