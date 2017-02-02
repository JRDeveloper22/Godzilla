using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DelegateEventTest))]
public class DelegateEventTestEditor : Editor {

    public override void OnInspectorGUI()
    {
        DelegateEventTest Mytarget = (DelegateEventTest)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("StartEngineTest"))
        {
            Mytarget.StartEngineTest();
        }
        if (GUILayout.Button("DelMessageCallbackTest"))
        {
            Mytarget.DelMessageCallbackTest();
        }
        if (GUILayout.Button("DelMessageTestFunc2"))
        {
            Mytarget.DelMessageTestFunc2();
        }
        if (GUILayout.Button("EventTest"))
        {
            Mytarget.EventTest();
        }
        if (GUILayout.Button("GMessageTest"))
        {
            Mytarget.GMessageTest();
        }
        if (GUILayout.Button("ShapEventTest"))
        {
            Mytarget.ShapEventTest();
        }
        if (GUILayout.Button("ShapContainerTest"))
        {
            Mytarget.ShapContainerTest();
        }
    }

}
