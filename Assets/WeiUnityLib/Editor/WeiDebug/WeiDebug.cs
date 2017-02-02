using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class WeiDebug : EditorWindow
{
    [MenuItem("WeiDebug/WeiDebug_Call_StackInfo")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(WeiDebug));
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Reset Call Stack"))
        {
            WeiDEBUG.stackIndex = 0;
            Debug.Log("statckIndex 0");
        }
    }
}
