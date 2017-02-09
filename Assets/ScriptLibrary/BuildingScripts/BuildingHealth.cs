using UnityEngine;
using System.Collections;

public class BuildingHealth : MonoBehaviour
{
    public bool hit = false;
    public float Damage = 100;
    public BuildingAtributes DB;

    [HideInInspector]
    public bool bePicked = false;

    
    public int holderPlayerIndex;
    
    public int otherplayer = 0;

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
        bePicked = false;
        onRgTime = onRgMaxTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        Player p = other.transform.GetComponent<Player>();
        if (p)
        {
            
            DB.MinusHealth(Damage);
            Debug.Log(p.name);
            Debug.Log(p.playerIndex);
            Debug.Log(otherplayer);
            if (DB.BuildingHealth <= 0)
            {
                Destroy(this);
            }
            if (p.playerIndex == otherplayer)
            {


                p.TakeDamage(10);
            }
        }
       
    }
}

