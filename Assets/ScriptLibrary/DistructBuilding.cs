using UnityEngine;
using System.Collections;

public class DistructBuilding : MonoBehaviour {
    public float BuildingHealth = 0;
    public GameObject prefab;
    public GameObject Box;
    private float ExposionPower = 10.0f;
    private float ExplosionRadius = 5.0f;
    Rigidbody Rbox;
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
            Rbox = gameObject.GetComponent<Rigidbody>();
            Rbox.useGravity = true;
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
        rb.AddExplosionForce(ExposionPower,explosionPos, ExplosionRadius,0.2f);

    }
    public void MinusHealth(float amount)
    {
        BuildingHealth -= amount;
    }
    IEnumerator Despawner()
    {
        
        yield return new WaitForSeconds(1);
        //make the dissolve shader
        Destroy(gameObject);
    }
}
