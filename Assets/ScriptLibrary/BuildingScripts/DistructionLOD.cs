using UnityEngine;
using System.Collections;

public class DistructionLOD : MonoBehaviour
{
    public BuildingAtributes BA;
    public GameObject prefab;
    public GameObject OriginPos;
    void Start()
    {
        BA = gameObject.GetComponent<BuildingAtributes>();
    }
    void OnCollisionEnter(Collision other)
    {
       if(other.transform.tag == "Player" && BA.BuildingHealth == 0)
        {
            GameObject DistrBuilding = (GameObject)Instantiate(prefab, OriginPos.transform.position, OriginPos.transform.rotation);

            Destroy(gameObject);
        }
    }

}
