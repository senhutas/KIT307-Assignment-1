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
    public GameObject armJoint1;
    public GameObject armJoint2;
    public GameObject armJoint3;
    public GameObject arm1;
    public GameObject arm2;
    public GameObject arm3;

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

        // Create arm profile for, well arms I guess.
        Vector3[] armProfile = new Vector3[]
        {
            // Right end of arm.
            new Vector3( 0.90f, -0.30f, 0.0f),
            new Vector3( 1.10f, -0.22f, 0.0f),
            new Vector3( 1.20f, -0.10f, 0.0f),
            new Vector3( 1.20f,  0.10f, 0.0f),
            new Vector3( 1.10f,  0.22f, 0.0f),
            new Vector3( 0.90f,  0.30f, 0.0f),

            // Left end of arm.
            new Vector3(-0.90f,  0.30f, 0.0f),
            new Vector3(-1.10f,  0.22f, 0.0f),
            new Vector3(-1.20f,  0.10f, 0.0f),
            new Vector3(-1.20f, -0.10f, 0.0f),
            new Vector3(-1.10f, -0.22f, 0.0f),
            new Vector3(-0.90f, -0.30f, 0.0f)
        };

        // Create the path for the arm.
        // Both the profile and path are very similar to the lamp work in tutorial.
        Matrix4x4[] armPath = new Matrix4x4[10];
        armPath[0] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1.0f)) * 
                     Matrix4x4.Translate(new Vector3(0.0f, 0.0f, -0.20f));
        armPath[1] = Matrix4x4.Scale(new Vector3(0.9f, 0.8f, 1.0f)) * 
                     Matrix4x4.Translate(new Vector3(0.0f, 0.0f, -0.20f));
        armPath[2] = Matrix4x4.Scale(new Vector3(0.9f, 0.8f, 1.0f)) * 
                     Matrix4x4.Translate(new Vector3(0.0f, 0.0f, -0.20f));
        armPath[3] = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, -0.15f));
        armPath[4] = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, -0.15f));
        armPath[5] = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.15f));
        armPath[6] = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.15f));
        armPath[7] = Matrix4x4.Scale(new Vector3(0.9f, 0.8f, 1.0f)) * 
                     Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.20f));
        armPath[8] = Matrix4x4.Scale(new Vector3(0.9f, 0.8f, 1.0f)) * 
                     Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.20f));
        armPath[9] = Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 1.0f)) * 
                     Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.20f));

        // Create the joints for the arm to rotate around.
        armJoint1 = new GameObject("ArmJoint1");
        armJoint1.transform.parent = armMount.transform;
        armJoint1.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
        armJoint1.transform.localRotation = Quaternion.Euler(0f, 0f, 35f);
        
        armJoint2 = new GameObject("ArmJoint2");
        armJoint2.transform.parent = armJoint1.transform;
        armJoint2.transform.localPosition = new Vector3(1.725f, 0.0f, 0.0f);    // AI helped give x values so I could line them up the way I wanted.
        armJoint2.transform.localRotation = Quaternion.Euler(0f, 0f, -35f);

        armJoint3 = new GameObject("ArmJoint3");
        armJoint3.transform.parent = armJoint2.transform;
        armJoint3.transform.localPosition = new Vector3(1.725f, 0.0f, 0.0f);    // AI helped give x values so I could line them up the way I wanted.
        armJoint3.transform.localRotation = Quaternion.Euler(0f, 0f, -35f);

        // Create the first arm.
        arm1 = new GameObject();
        arm1.name = "Arm1";

        arm1.AddComponent<MeshFilter>().mesh = MeshUtilities.Sweep(armProfile, armPath, false);
        arm1.AddComponent<MeshRenderer>().sharedMaterial = doubleSidedMaterial;

        // Attach the first arm to the first arm joint.
        arm1.transform.parent = armJoint1.transform;
        arm1.transform.localPosition = new Vector3(0.8625f, 0.0f, 0.0f);    // AI helped give x values so I could line them up the way I wanted.
        arm1.transform.localRotation = Quaternion.identity;

        // Create and attach the second arm to the second arm joint.
        arm2 = Instantiate(arm1, armJoint2.transform);
        arm2.name = "Arm2";
        arm2.transform.localPosition = new Vector3(0.8625f, 0.0f, 0.0f);    // AI helped give x values so I could line them up the way I wanted.
        arm2.transform.localRotation = Quaternion.identity;

        // Create and attach the third arm to the third arm joint.
        arm3 = Instantiate(arm1, armJoint3.transform);
        arm3.name = "Arm3";
        arm3.transform.localPosition = new Vector3(0.8625f, 0.0f, 0.0f);    // AI helped give x values so I could line them up the way I wanted.
        arm3.transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}