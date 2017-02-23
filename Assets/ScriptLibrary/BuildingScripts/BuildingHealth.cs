using UnityEngine;
using System.Collections;

public class BuildingHealth : MonoBehaviour
{
    public bool hit = false;
    public float Damage = 100;
    public BuildingAtributes DB;

    [HideInInspector]
    public bool bePicked = false;

    [HideInInspector]
    public bool canDoDamage = false;
    
    public int holderPlayerIndex;
    
    public int otherplayer = -1;

    public float onRgMaxTime = 5.0f;
    float onRgTime;
    //public bool isPicked = false;
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
    private void Update()
    {
        if (!bePicked)
        {
            if (onRgTime > 0) { onRgTime -= Time.deltaTime; return; }
            if (GetComponent<Rigidbody>()) {
                //Destroy(GetComponent<Rigidbody>());
            }
        }
    }

    public void BeThrowed()
    {
        //bePicked = false;
        canDoDamage = true;
        onRgTime = onRgMaxTime;
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
                //float doDamage = GetComponent<Rigidbody>().velocity.magnitude;
                p.TakeDamage(10);
            }
        }
       
    }

    void ResetInfo()
    {
        otherplayer = -1;
    }
    
}

