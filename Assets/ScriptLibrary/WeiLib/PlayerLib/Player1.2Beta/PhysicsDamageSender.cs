using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     we Add collider and Rigidbody 
/// </summary>
public class PhysicsDamageSender : MonoBehaviour {

    public Transform targetTF;

    private void LateUpdate()
    {
        transform.position = targetTF.position;
        transform.rotation = targetTF.rotation;
    }

    public virtual void SenderCallBack(PhysicsDamangeResiver resiver)
    {
        Debug.Log("resiver do Action 1");
        Debug.Log("resiver do Something 2");
        Debug.Log("resiver do Something 3");
    }

    private void OnCollisionEnter(Collision collision)
    {
       
		PhysicsDamangeResiver resiver = collision.gameObject.GetComponent<PhysicsDamangeResiver>();
        if (resiver != null)
        {
            Debug.Log("Demage To Resiver");
        }
    }
}
