using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Space))]
public class SpaceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Space myScript = (Space)target;
        if(GUILayout.Button("Update Name"))
        {
            myScript.UpdateName();
        }
    }
}

