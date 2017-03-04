using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuildAtr : MonoBehaviour {
    public bool Taken;
    public DistructionLOD Main;
    void Update()
    {
        Main = gameObject.GetComponentInChildren<DistructionLOD>();
        if (Main == null)
        {
            Taken = true;
        }
    }
}
