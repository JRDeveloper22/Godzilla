using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AttributesTest))]
public class AttributeTestEditor : Editor {

    public override void OnInspectorGUI()
    {
        AttributesTest MyTarget = (AttributesTest)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("MainClass_MainTest"))
        {
            AttributesTest.MainClass.MainTest();
        }
    }

}
