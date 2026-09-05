using UnityEngine;

public class HoleInTheWallMaker : MonoBehaviour
{
    public Material doubleSidedMaterial;   // The double-sided material.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = doubleSidedMaterial;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        meshFilter.mesh = MeshUtilities.WallWithHole(new Vector3(5.0f, 2.5f, 0.5f), new Vector2(2.5f, 2f), 0.25f, 16);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
