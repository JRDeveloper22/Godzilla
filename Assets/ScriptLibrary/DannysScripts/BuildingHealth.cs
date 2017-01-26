using UnityEngine;
using System.Collections;

public class BuildingHealth : MonoBehaviour {

    public float healthAmount = 100;
    public  DistructBuilding DB;
    void Start()
    {
        DB = gameObject.GetComponent<DistructBuilding>();
        //Debug.Log(DB.name);
        if (DB == null)
        {
            Debug.LogError(gameObject.name);
            Debug.LogError(": does not have a DistructBuilding Script");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DB.MinusHealth(healthAmount);
            //Debug.Log(DB.name);
            if (DB.BuildingHealth <= 0)
            {
                Destroy(this);
            }
        }
    }
}
