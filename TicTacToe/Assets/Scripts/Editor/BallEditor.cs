using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Ball))]
public class BallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Ball myScript = (Ball)target;
        if(GUILayout.Button("Update Material"))
        {
            myScript.SetMaterial();
        }
    }
}

