using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKObjectPhysics : MonoBehaviour {

    public float damageScale = 4.0f;
    public string ignoreTag = "";
    
    Rigidbody rg;
    private void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.transform.GetComponent<Player>().playerID == playerIgnore) return;
        if (collision.transform.root.tag == ignoreTag) return;

        IDamageable idagageable = collision.transform.GetComponent<IDamageable>();
        if (idagageable != null)
        {
            idagageable.TakeDamage(rg.velocity.magnitude * damageScale);
            this.enabled = true;
        }
    }
}
