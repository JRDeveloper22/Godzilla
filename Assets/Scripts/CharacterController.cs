using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    public float inputDelay = .01f;
    public float ForwardVel = 12;
    public float rotateVel = 100;
    Quaternion targetRotation;
    Rigidbody rbody;
    float forwardInput, turnInput;
    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    void Start ()
    {
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
            rbody = GetComponent<Rigidbody>();
        else
            Debug.Log("This characther needs a rigidbody");
        forwardInput = turnInput = 0;
	}
    void GetInput()
    {
        //W And S keys also LYAxis
        //forwardInput = Input.GetAxis("Vertical");
        forwardInput = Input.GetAxis("LXAxis");

        //A and D keys also lXAxis Will change later
        //turnInput = Input.GetAxis("Horizontal");
        turnInput = Input.GetAxis("RXAxis");

    }
	
	// Update is called once per frame
	void Update () {
        GetInput();
        Turn();
	}
    void FixedUpdate()
    {
        Run();
    }
    void Run()
    {
        if (Mathf.Abs(forwardInput) > inputDelay)
        {
            //move
            rbody.velocity = transform.forward * forwardInput * ForwardVel;
        }
        else
            rbody.velocity = Vector3.zero;
    }
    void Turn()
    {
        if (Mathf.Abs(turnInput) > inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);
        }

        transform.rotation = targetRotation;
    }
}
