using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrafebPool : MonoBehaviour
{
    public GameObject[] ParticlePrafeb;
	public bool myDebug = false;
    public int instanceNumber;
	

	Dictionary<string, List<GameObject>> prafebsDic = new Dictionary<string, List<GameObject>>();

	string instanceSuffix = "_Clone";

	public static PrafebPool Managerinstance = null;

	//Make a singleTon
	void Awake(){
		if(Managerinstance != null){
			Destroy(this);
		}
		Managerinstance = this;
	}

	// Use this for initialization
	void Start () {

        for (int i = 0; i < ParticlePrafeb.Length; ++i) {

            string prafebName = ParticlePrafeb[i].name;
			prafebsDic[prafebName] = new List<GameObject>();
            GameObject instancePool = new GameObject();
            instancePool.name = prafebName + "Pool";

            for (int j = 0; j < instanceNumber; ++j) {
				GameObject instance = (GameObject)Instantiate(ParticlePrafeb[i]);
                SetUpProperty(instance, prafebName, instancePool.transform);
            }
        }
	}

    private void SetUpProperty(GameObject instance,string prafebName,Transform parent) {
        instance.GetComponent<Renderer>().material.color = Color.red;
        instance.transform.SetParent(parent);
        instance.SetActive(false);
        instance.name = prafebName + instanceSuffix;
        prafebsDic[prafebName].Add(instance);
    }

	public GameObject GetInstanceByPrefabIndex(int index){

		if(0 > index || index >= ParticlePrafeb.Length){
			if(myDebug)
				Debug.Log("PrafebPool GetInstanceByPrefabIndex failed..... ");
			return new GameObject();
		}

		string name = ParticlePrafeb[index].name;

		if(prafebsDic[name].Count >0){
			GameObject bullet = prafebsDic[name][0];
			bullet.SetActive(true);
			prafebsDic[name].RemoveAt(0);
			return bullet;
		}else{
			GameObject instance = (GameObject)Instantiate(ParticlePrafeb[index]);
            instance.name = name + instanceSuffix;
			return instance;

		}

	}

	public void ReturnPrafeb(GameObject instance){
		int poolNameLength = instance.name.Length - instanceSuffix.Length;
		string poolName = instance.name.Substring(0,poolNameLength);
		instance.SetActive(false);
		prafebsDic[poolName].Add(instance);

		if(myDebug)
			Debug.Log("BulletManager ReturnByllet Function was Called");
	}

	public static PrafebPool GetInstance(){
		return Managerinstance;
	}

}
