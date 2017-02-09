using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour {

    Ray ray;
    [HideInInspector]
    public bool isGround = false;

    [SerializeField]
    private float rayLength = 0.3f ;

    private void FixedUpdate()
    {

        if (Physics.Raycast(transform.position, Vector3.down, rayLength))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
            //Debug.Log("is Ground False");
        }
    }
}
