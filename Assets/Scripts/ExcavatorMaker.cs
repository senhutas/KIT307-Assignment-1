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
    public GameObject wheelJoint1;
    public GameObject wheelJoint2;
    public GameObject wheelJoint3;
    public GameObject wheelJoint4;
    public GameObject axle1;
    public GameObject axle2;
    public GameObject axle3;
    public GameObject axle4;
    public GameObject wheel1;
    public GameObject wheel2;
    public GameObject wheel3;
    public GameObject wheel4;

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
                           Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 0.0f));
        wheelBasePath[1] = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, 0.0f)) *
                           Matrix4x4.Scale(new Vector3(2.3f, 0.4f, 1.6f));
        wheelBasePath[2] = Matrix4x4.Translate(new Vector3(0.0f, 0.4f, 0.0f)) *
                           Matrix4x4.Scale(new Vector3(2.3f, 0.4f, 1.6f));
        wheelBasePath[3] = Matrix4x4.Translate(new Vector3(0.0f, 0.4f, 0.0f)) *
                           Matrix4x4.Scale(new Vector3(0.0f, 0.0f, 0.0f));

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
        turntable.transform.localPosition = new Vector3(0.0f, 0.10f, 0.0f);

        turntable.AddComponent<MeshFilter>().mesh = MeshUtilities.Cylinder(16, 0.3f, 0.10f);
        turntable.AddComponent<MeshRenderer>().sharedMaterial = doubleSidedMaterial;

        // Create the body of the excavator.
        body = new GameObject();
        body.name = "Body";
        body.transform.parent = turntableJoint.transform;
        body.transform.localPosition = new Vector3(0, 0.20f, 0f);

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

        // Create the joints for the wheels (technically axles).
        wheelJoint1 = new GameObject("WheelJoint1");
        wheelJoint1.transform.parent = wheelBase.transform;
        wheelJoint1.transform.localPosition = new Vector3(0.80f, 0.2f, 0.8f);
        wheelJoint1.transform.localRotation = Quaternion.identity;

        wheelJoint2 = new GameObject("WheelJoint2");
        wheelJoint2.transform.parent = wheelBase.transform;
        wheelJoint2.transform.localPosition = new Vector3(-0.80f, 0.2f, 0.8f);
        wheelJoint2.transform.localRotation = Quaternion.identity;

        wheelJoint3 = new GameObject("WheelJoint3");
        wheelJoint3.transform.parent = wheelBase.transform;
        wheelJoint3.transform.localPosition = new Vector3(0.80f, 0.2f, -0.8f);
        wheelJoint3.transform.localRotation = Quaternion.identity;

        wheelJoint4 = new GameObject("WheelJoint4");
        wheelJoint4.transform.parent = wheelBase.transform;
        wheelJoint4.transform.localPosition = new Vector3(-0.80f, 0.2f, -0.8f);
        wheelJoint4.transform.localRotation = Quaternion.identity;

        // Create the first axle and connect it to the wheel base (well, the joint).
        axle1 = new GameObject("Axle1");
        axle1.transform.parent = wheelJoint1.transform;
        axle1.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        axle1.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        axle1.AddComponent<MeshFilter>().mesh = MeshUtilities.Cylinder(16, 0.08f, 0.1f);
        axle1.AddComponent<MeshRenderer>().sharedMaterial = doubleSidedMaterial;

        // Create the second axle and connect it to the wheel base (well, the joint).
        axle2 = Instantiate(axle1, wheelJoint2.transform);
        axle2.name = "Axle2";
        axle2.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        axle2.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        // Create the third axle and connect it to the wheel base (well, the joint).
        axle3 = Instantiate(axle1, wheelJoint3.transform);
        axle3.name = "Axle3";
        axle3.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        axle3.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);

        // Create the fourth axle and connect it to the wheel base (well, the joint).
        axle4 = Instantiate(axle1, wheelJoint4.transform);
        axle4.name = "Axle4";
        axle4.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        axle4.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);

        // Create the first wheel and attach it to the associated axle.
        wheel1 = new GameObject("Wheel1");
        wheel1.transform.parent = axle1.transform;
        wheel1.transform.localPosition = new Vector3(0.0f, 0.20f, 0.0f);
        wheel1.transform.localRotation = Quaternion.identity;

        wheel1.AddComponent<MeshFilter>().mesh = MeshUtilities.Cylinder(16, 0.375f, 0.15f);
        wheel1.AddComponent<MeshRenderer>().sharedMaterial = doubleSidedMaterial;

        // Create the second wheel and attach it to the associated axle.
        wheel2 = Instantiate(wheel1, axle2.transform);
        wheel2.name = "Wheel2";
        wheel2.transform.localPosition = new Vector3(0.0f, 0.20f, -0.0f);
        wheel2.transform.localRotation = Quaternion.identity;

        // Create the third wheel and attach it to the associated axle.
        wheel3 = Instantiate(wheel1, axle3.transform);
        wheel3.name = "Wheel3";
        wheel3.transform.localPosition = new Vector3(0.0f, 0.20f, 0.0f);
        wheel3.transform.localRotation = Quaternion.identity;

        // Create the fourth wheel and attach it to the associated axle.
        wheel4 = Instantiate(wheel1, axle4.transform);
        wheel4.name = "Wheel4";
        wheel4.transform.localPosition = new Vector3(0.0f, 0.20f, 0.0f);
        wheel4.transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}