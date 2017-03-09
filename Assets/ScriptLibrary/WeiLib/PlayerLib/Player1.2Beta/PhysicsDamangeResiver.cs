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
    public Test1_2.Player1_2 player;

    // Use this for initialization
    void Start () {
        player = transform.root.GetComponent<Test1_2.Player1_2>();
	}
    public void ResiveDamage(DamageEvent damEvent)
    {
        player.TakeDamage(damEvent.damage);
    }
}

public struct DamageEvent
{
    public float damage;
    public DamageEvent(float _damage)
    {
        damage = _damage;
    }
    //WhatEver other effect. Like the get hit animation Effect.
    //...
    //...
}
