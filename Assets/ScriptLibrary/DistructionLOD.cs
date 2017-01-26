using UnityEngine;
using System.Collections;

public class DistructionLOD : MonoBehaviour
{
    public GameObject prefab;
    public GameObject OriginPos;
    void OnTriggerEnter(Collider other)
    {
       if(other.tag == "Player")
        {
            GameObject DistrBuilding = (GameObject)Instantiate(prefab, OriginPos.transform.position, OriginPos.transform.rotation);

            Destroy(gameObject);
        }
    }

}
