using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity {

    PlayerController pController;

    public GameObject pickUpHandler;
    public float pickDistance;
    public LayerMask pickUpLayer;
    public KeyCode pickKey = KeyCode.E;
    public float ThrowForce = 2;
    // Use this for initialization
    public override void Start () {
        base.Start();
        pController = GetComponent<PlayerController>();
        pController.InitCamera();
        pController.InitAnimation();
        pController.InitRigidBody();
	}
	
	// Update is called once per frame
	void Update () {
        pController.isRunning = Input.GetKey(KeyCode.LeftShift);
        pController.GenericMotion();

        if (Input.GetMouseButton(0))
        {
            pController.PunchAnimation();
        }

        if (Input.GetKeyDown(pickKey))
        {
            if (pickUpHandler.transform.childCount > 0)
                ThrowBuilding();
            else
                PickTheBuilding();

        }

	}

    private void FixedUpdate()
    {
        pController.UpdateRigidBodyController();
    }

    void PickTheBuilding()
    {
       Collider[] cs =  Physics.OverlapSphere(transform.position, pickDistance, pickUpLayer);
       
       if(cs.Length >0)
            cs[0].transform.parent.parent = pickUpHandler.transform;

    }
    void ThrowBuilding()
    {
        GameObject pickUp = pickUpHandler.transform.GetChild(0).gameObject;
        if (pickUp != null)
        {
            pickUp.transform.parent = null;
            Rigidbody rPickup;
            if (!pickUp.GetComponent<Rigidbody>())
            {
                rPickup = pickUp.AddComponent<Rigidbody>();
                rPickup.useGravity = true;
            }
            else {
                rPickup = pickUp.GetComponent<Rigidbody>();
            }
            rPickup.AddForce(transform.forward * ThrowForce);
        }
    }
}
