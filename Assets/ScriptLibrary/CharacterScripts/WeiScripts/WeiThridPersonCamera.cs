using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeiThridPersonCamera : MonoBehaviour {

    /*
    public KeyCode yawLeft;
    public KeyCode yawRight;
    public KeyCode pitchUp;
    public KeyCode pitchDown;*/

    public Vector2 pitchMinMax = new Vector2(0, 85);
    public float rotationSmoothTime = 0.12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public Transform target;
    public Vector2 rangeToTarget = new Vector2(2, 20);
    float dstToTarget = 10;
    public float cameraMoveSensitivity = 10;
    float yaw;  //Rotation around Y Axis
    float pitch = 75;//Rotation around X Axis
    float zoomInOut;

	// Update is called once per frame
	void LateUpdate () {

#if UNITY_XBOXONE
        //Whatever how will you rotate the camera
#else     
      /*if (Input.GetKey(yawLeft)) yaw -= cameraMoveSensitivity * Time.deltaTime;
        if (Input.GetKey(yawRight)) yaw += cameraMoveSensitivity * Time.deltaTime;
        if (Input.GetKey(pitchUp)) pitch += cameraMoveSensitivity * Time.deltaTime;
        if (Input.GetKey(pitchDown)) pitch -= cameraMoveSensitivity * Time.deltaTime;*/

        yaw += Input.GetAxis("Mouse X") * cameraMoveSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * cameraMoveSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        dstToTarget += Input.GetAxis("Mouse ScrollWheel");
        dstToTarget = Mathf.Clamp(dstToTarget, rangeToTarget.x, rangeToTarget.y);
#endif
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;
        transform.position = target.position - transform.forward * dstToTarget;
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(10,60, 240, 20), "scroll middle Mouse to zoom in/out");
    }
}
