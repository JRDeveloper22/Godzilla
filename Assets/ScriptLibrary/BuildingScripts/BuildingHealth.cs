﻿using UnityEngine;
using System.Collections;

public class BuildingHealth : MonoBehaviour
{
    public bool hit = false;
    public float Damage = 100;
    public BuildingAtributes DB;
    
    public int holderPlayerIndex;

    public bool canDoDamage = false;
    public int otherplayer = -1;

    void Start()
    {
        DB = gameObject.GetComponent<BuildingAtributes>();
        //Debug.Log(DB.name);
        if (DB == null)
        {
            Debug.LogError(gameObject.name);
            Debug.LogError(": does not have a DistructBuilding Script");
        }
    }

    public void BeThrowed()
    {
        canDoDamage = true;
        Invoke("ResetInfo", 0.5f);
    }

    private void OnCollisionEnter(Collision other)
    {
        Player p = other.transform.GetComponent<Player>();

        if (p)
        {
            //building Get Damage
            DB.MinusHealth(Damage);

            if (DB.BuildingHealth <= 0)
            {
                Destroy(this);
            }
            //If hit other player
            if (p.playerIndex == otherplayer)
            {
                float doDamage = GetComponent<Rigidbody>() ? GetComponent<Rigidbody>().velocity.magnitude : 0;
                p.TakeDamage(doDamage);
            }
        }
       
    }

    void ResetInfo()
    {
        otherplayer = -1;
        canDoDamage = false;
    }
    
}

