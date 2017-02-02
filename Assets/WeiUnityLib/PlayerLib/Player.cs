using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity {

    PlayerController pController;

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
	}

    private void FixedUpdate()
    {
        pController.UpdateRigidBodyController();
    }
}
