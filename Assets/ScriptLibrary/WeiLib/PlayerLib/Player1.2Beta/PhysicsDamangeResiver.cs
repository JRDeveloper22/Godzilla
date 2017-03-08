using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     In Thte tearm Project Godzilla, When I impliment the Fightting damage system.
/// if I wanna use Physics based fighting System.
///     1. 
///         Only the main body part add Collider and Rigidbody.
///         the arm leg and head ...else use only Collider or Trigger....So we only do damage when 
///         we punch or kick the main body.
///     2.
///         each parts of the body has rigidbody and Collider.  we use joint to link them up
///         So We can use PhysicsBasedDamage system on the whole body.
///         
/// </summary>
public class PhysicsDamangeResiver : MonoBehaviour {

    [HideInInspector]
    public Test1_2.PlayerController1_2 playerController;

    // Use this for initialization
    void Start () {
        playerController = transform.root.GetComponent<Test1_2.PlayerController1_2>();
	}


}
