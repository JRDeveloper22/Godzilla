using UnityEngine;
using System.Collections;

public class DistructBuilding : MonoBehaviour {
    public float BuildingHealth = 0;
    public GameObject prefab;
    public GameObject Box;
    private float ExposionPower = 20.0f;
    private float ExplosionRadius = 5.0f;
    void Start ()
    {
        Box = this.gameObject;
    }

	void Update ()
    {
        if (BuildingHealth > 0)
        {
            
            RemoveRigidBody();
        }
        else
        {
            AddRigidBody();
            AddExpolison();
            this.transform.parent = null;
            StartCoroutine(Despawner());
        }
    }
    void AddRigidBody()
    {
        if(!Box.GetComponent<Rigidbody>())
        {
            Box.AddComponent<Rigidbody>();
            GameObject smoke = (GameObject)Instantiate(prefab, Box.transform.position, Box.transform.rotation);
            Destroy(smoke, 2.0f);
        }
    }
    void RemoveRigidBody()
    {
        if(Box.GetComponent<Rigidbody>())
        {
            Destroy(Box.GetComponent<Rigidbody>());
        }
    }
    void AddExpolison()
    {
        Vector3 explosionPos = transform.position;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
            Debug.Log("there is no rigidbody on this gameobject");
        rb.AddExplosionForce(ExposionPower,explosionPos, ExplosionRadius,3.0f);

    }
    public void MinusHealth(float amount)
    {
        BuildingHealth -= amount;
    }
    IEnumerator Despawner()
    {
        
        yield return new WaitForSeconds(15);
        //make the dissolve shader
        Destroy(gameObject);
    }
}
