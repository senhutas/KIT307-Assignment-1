using UnityEngine;

public class ExcavatorMaker : MonoBehaviour
{
    public Material doubleSidedMaterial;   // The double-sided material.

    // Member variables for the excavator hierarchy.
    public GameObject wheelBase;
    public GameObject turntableJoint;
    public GameObject turntable;
    public GameObject body;
    public GameObject cab;
    public GameObject armMount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create a square profile, with duplicated vertices so that the edges are sharp.
        Vector3[] squareProfile = new Vector3[]
        {
            new Vector3(-0.5f, 0.0f, -0.5f),
            new Vector3(-0.5f, 0.0f, -0.5f),
            new Vector3(-0.5f, 0.0f,  0.5f),
            new Vector3(-0.5f, 0.0f,  0.5f),
            new Vector3( 0.5f, 0.0f,  0.5f),
            new Vector3( 0.5f, 0.0f,  0.5f),
            new Vector3( 0.5f, 0.0f, -0.5f), 
            new Vector3( 0.5f, 0.0f, -0.5f)
        };

        // Create the wheel base, the main parent of the excavator.
        wheelBase = new GameObject();
        wheelBase.name = "WheelBase";
        wheelBase.transform.parent = transform;
        wheelBase.transform.localPosition = new Vector3(0, 0, 0);

        Matrix4x4[] wheelBasePath = new Matrix4x4[4];
        wheelBasePath[0] = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.0f)) *
                           Matrix4x4.Scale(new Vector3(0, 0, 0));
        wheelBasePath[1] = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.0f)) *
                           Matrix4x4.Scale(new Vector3(2.0f, 0.4f, 2.0f));
        wheelBasePath[2] = Matrix4x4.Translate(new Vector3(0.0f, 0.4f, 0.0f)) *
                           Matrix4x4.Scale(new Vector3(2.0f, 0.4f, 2.0f));
        wheelBasePath[3] = Matrix4x4.Translate(new Vector3(0.0f, 0.4f, 0.0f)) *
                           Matrix4x4.Scale(new Vector3(0, 0, 0));

        wheelBase.AddComponent<MeshFilter>().mesh = MeshUtilities.Sweep(squareProfile, wheelBasePath, true);
        wheelBase.AddComponent<MeshRenderer>().sharedMaterial = doubleSidedMaterial;

        // Create the main turntable joint for the body of the excavator.
        turntableJoint = new GameObject();
        turntableJoint.name = "TurntableJoint";
        turntableJoint.transform.parent = wheelBase.transform;
        turntableJoint.transform.localPosition = new Vector3(0.0f, 0.4f, 0.0f);

        // Create the shaft that connects the body and the wheel base.
        // AI helped me position the cylinder for the turntable because I forgot the cyliner function goes out in both
        // directions from the centre. Once it explained that to me I was able to do the maths and figure the rest out.
        turntable = new GameObject();
        turntable.name = "Turntable";
        turntable.transform.parent = turntableJoint.transform;
        turntable.transform.localPosition = new Vector3(0.0f, 0.15f, 0.0f);

        turntable.AddComponent<MeshFilter>().mesh = MeshUtilities.Cylinder(16, 0.3f, 0.15f);
        turntable.AddComponent<MeshRenderer>().sharedMaterial = doubleSidedMaterial;

        // Create the body of the excavator.
        body = new GameObject();
        body.name = "Body";
        body.transform.parent = turntableJoint.transform;
        body.transform.localPosition = new Vector3(0, 0.3f, 0f);

        Matrix4x4[] bodyPath = new Matrix4x4[4];
        bodyPath[0] = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.0f)) *
                      Matrix4x4.Scale(new Vector3(0, 0, 0));
        bodyPath[1] = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.0f)) *
                      Matrix4x4.Scale(new Vector3(2.5f, 1.0f, 2.5f));
        bodyPath[2] = Matrix4x4.Translate(new Vector3(0.0f, 0.8f, 0.0f)) *
                      Matrix4x4.Scale(new Vector3(2.5f, 1.0f, 2.5f));
        bodyPath[3] = Matrix4x4.Translate(new Vector3(0.0f, 0.8f, 0.0f)) *
                      Matrix4x4.Scale(new Vector3(0, 0, 0));

        body.AddComponent<MeshFilter>().mesh = MeshUtilities.Sweep(squareProfile, bodyPath, true);
        body.AddComponent<MeshRenderer>().sharedMaterial = doubleSidedMaterial;

        // Create the cab, for the little human to sit in (theoretically).
        cab = new GameObject();
        cab.name = "Cab";
        cab.transform.parent = body.transform;
        cab.transform.localPosition = new Vector3(0.45f, 0.8f, 0.50f);

        Matrix4x4[] cabPath = new Matrix4x4[4];
        cabPath[0] = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.0f)) *
                     Matrix4x4.Scale(new Vector3(0, 0, 0));
        cabPath[1] = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.0f)) *
                     Matrix4x4.Scale(new Vector3(1.25f, 1.0f, 1.0f));
        cabPath[2] = Matrix4x4.Translate(new Vector3(0.0f, 1.3f, 0.0f)) *
                     Matrix4x4.Scale(new Vector3(1.25f, 1.0f, 1.0f));
        cabPath[3] = Matrix4x4.Translate(new Vector3(0.0f, 1.3f, 0.0f)) *
                     Matrix4x4.Scale(new Vector3(0, 0, 0));

        cab.AddComponent<MeshFilter>().mesh = MeshUtilities.Sweep(squareProfile, cabPath, true);
        cab.AddComponent<MeshRenderer>().sharedMaterial = doubleSidedMaterial;

        // Create the mounting bracket for the arms to connect to.
        armMount = new GameObject();
        armMount.name = "ArmMount";
        armMount.transform.parent = body.transform;
        armMount.transform.localPosition = new Vector3(0.45f, 0.8f, -0.65f);

        // Used AI to be a calculator to do the mapths to scale the 2nd rectangle part evently, I'm sorry, this is lazy, but I feel
        // like I'm running out of time, so as you encouraged within the tutorial, I let AI pick the values.
        Matrix4x4[] armMountPath = new Matrix4x4[6];
        armMountPath[0] = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.0f)) *
                          Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 0.0f));
        armMountPath[1] = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.0f)) *
                          Matrix4x4.Scale(new Vector3(0.8f, 1.0f, 0.5f));
        armMountPath[2] = Matrix4x4.Translate(new Vector3(0.0f, 0.1f, 0.0f)) *
                          Matrix4x4.Scale(new Vector3(0.8f, 1.0f, 0.5f));
        armMountPath[3] = Matrix4x4.Translate(new Vector3(0.0f, 0.1f, 0.0f)) *
                          Matrix4x4.Scale(new Vector3(0.6f, 1.0f, 0.3f));
        armMountPath[4] = Matrix4x4.Translate(new Vector3(0.0f, 0.5f, 0.0f)) *
                          Matrix4x4.Scale(new Vector3(0.45f, 1.0f, 0.25f));
        armMountPath[5] = Matrix4x4.Translate(new Vector3(0.0f, 0.5f, 0.0f)) *
                          Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 0.0f));

        armMount.AddComponent<MeshFilter>().mesh = MeshUtilities.Sweep(squareProfile, armMountPath, true);
        armMount.AddComponent<MeshRenderer>().sharedMaterial = doubleSidedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}