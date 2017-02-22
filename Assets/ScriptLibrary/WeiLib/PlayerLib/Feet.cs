using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Feet : MonoBehaviour
    {

        Ray ray;
        RaycastHit hitInfo;
        [HideInInspector]
        public bool isGround = false;
        [HideInInspector]
        public float distanceToGround;

        //[SerializeField]
        //private float rayLength = 0.3f;

        private void FixedUpdate()
        {

            if (Physics.Raycast(transform.position, Vector3.down,out hitInfo, 1000))
            {
                distanceToGround = (hitInfo.point - transform.position).magnitude;
                isGround = true;
            }
            else
            {
                isGround = false;
                //Debug.Log("is Ground False");
            }
        }
    }
