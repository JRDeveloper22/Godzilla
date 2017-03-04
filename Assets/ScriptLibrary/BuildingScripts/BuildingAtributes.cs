using UnityEngine;
using System.Collections;

public class BuildingAtributes : MonoBehaviour {
    public float BuildingHealth = 100;
    public MainBuildAtr Parent;
    void Start()
    {
        Parent = gameObject.GetComponentInParent<MainBuildAtr>();
    }
    public void MinusHealth(float amount)
    {
        BuildingHealth -= amount;
    }
}
