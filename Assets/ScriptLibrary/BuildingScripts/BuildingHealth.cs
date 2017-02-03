using UnityEngine;
using System.Collections;

public class BuildingHealth : MonoBehaviour
{
    public bool hit = false;
    public float Damage = 100;
    public BuildingAtributes DB;
    public GameObject RB;
    //public bool isPicked = false;
    void Start()
    {
        DB = gameObject.GetComponent<BuildingAtributes>();
        RB = GameObject.FindGameObjectWithTag("root");
        //Debug.Log(DB.name);
        if (DB == null)
        {
            Debug.LogError(gameObject.name);
            Debug.LogError(": does not have a DistructBuilding Script");
        }
    }
    private void OnCollisionEnter(Collision other)
    {

        if (other.transform.tag == "Player")
        {
            DB.MinusHealth(Damage);
            //Debug.Log(DB.name);
            if (DB.BuildingHealth <= 0)
            {
                Destroy(this);
            }
        }

    }
}

