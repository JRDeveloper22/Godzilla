using UnityEngine;
using System.Collections;

public class DistructBuilding : MonoBehaviour {
    public BuildingAtributes BA;
    public GameObject prefab;
    public GameObject Box;
    Rigidbody Rbox;
    float distToGround = 1;
    void Start ()
    {
        BA = gameObject.GetComponent<BuildingAtributes>();
        Box = this.gameObject;
    }

	void Update ()
    {
        
        if (BA.BuildingHealth > 0)
        {
            AddRigidBodyWC();
        }
        else
        {
            AddRigidBodyNC();
            this.transform.parent = null;
            StartCoroutine(Despawner(2f));
        }
    }
    void AddRigidBodyNC()
    {
        if(!Box.GetComponent<Rigidbody>())
        {
            Box.AddComponent<Rigidbody>();
            Rbox = gameObject.GetComponent<Rigidbody>();
            Rbox.useGravity = true;
        }
    }
    void AddRigidBodyWC()
    {
        if (!Box.GetComponent<Rigidbody>())
        {
            Box.AddComponent<Rigidbody>();
            Rbox = gameObject.GetComponent<Rigidbody>();
            Rbox.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            Rbox.useGravity = true;
        }
    }
    void RemoveRigidBody()
    {
        if(Box.GetComponent<Rigidbody>())
        {
            Destroy(Box.GetComponent<Rigidbody>());
        }
    }
    IEnumerator Despawner(float time)
    {
        
        yield return new WaitForSeconds(time);
        //make the dissolve shader
        Destroy(gameObject);
    }
}
