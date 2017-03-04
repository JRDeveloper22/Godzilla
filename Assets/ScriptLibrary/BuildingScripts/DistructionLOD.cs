using UnityEngine;
using System.Collections;

public class DistructionLOD : MonoBehaviour
{
    public BuildingAtributes BA;
    public GameObject prefab;
    void Start()
    {
        BA = gameObject.GetComponent<BuildingAtributes>();
    }
    void Update()
    {
       if(BA.BuildingHealth == 0)
        {
            GameObject DistrBuilding = (GameObject)Instantiate(prefab, BA.transform.position, BA.transform.rotation);
            Destroy(gameObject);
        }
    }
    /*private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Player")
        {
            GameObject DistrBuilding = (GameObject)Instantiate(prefab, BA.transform.position, BA.transform.rotation);
            Destroy(gameObject);
        }
    }*/
}
