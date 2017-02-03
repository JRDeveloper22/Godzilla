using UnityEngine;
using System.Collections;

public class BuildingHealth : MonoBehaviour
{
    public bool hit = false;
    public float Damage = 100;
    public BuildingAtributes DB;

    [HideInInspector]
    public bool bePicked = false;

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
                Destroy(GetComponent<Rigidbody>());
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

