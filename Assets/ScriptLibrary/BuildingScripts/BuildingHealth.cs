using UnityEngine;
using System.Collections;

public class BuildingHealth : MonoBehaviour {

    public float healthAmount = 100;
    public BuildingAtributes DB;
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
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("Hit");
            Debug.Log(DB.BuildingHealth);
            DB.MinusHealth(healthAmount);
            //Debug.Log(DB.name);
            if (DB.BuildingHealth <= 0)
            {
                Destroy(this);
            }
        }
    }
}
