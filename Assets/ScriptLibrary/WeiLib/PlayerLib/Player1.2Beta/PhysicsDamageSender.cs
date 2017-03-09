using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     we Add collider and Rigidbody 
/// </summary>
public class PhysicsDamageSender : MonoBehaviour {

    public Transform targetTF;
    public float damageAmount = 10f;

    Test1_2.PlayerController1_2 pc;
    private void Start()
    {
        pc = transform.root.GetComponent<Test1_2.PlayerController1_2>();
    }

    private void LateUpdate()
    {
        transform.position = targetTF.position;
        transform.rotation = targetTF.rotation;
    }

    public virtual void SenderCallBack(DamageCallBackInfo callbackInfo)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!pc.isAttacking) { return; }
		PhysicsDamangeResiver resiver = collision.gameObject.GetComponent<PhysicsDamangeResiver>();
        if (resiver != null)
        {
            resiver.ResiveDamage(new DamageEvent(damageAmount));
        }
    }
}

public struct DamageCallBackInfo
{
    int index1;
    public DamageCallBackInfo(int _index1)
    {
        index1 = _index1;
    }       
}

