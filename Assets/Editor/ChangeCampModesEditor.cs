using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChangeCampModes))]
public class ChangeCampModesEditor : Editor
{
    public ChangeCampModes current
    {
        get
        {
            return (ChangeCampModes)target;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach(GameObject s in current.structures)
        {
            if(GUILayout.Button("Open " + s.name))
            {
                current.SetStructure(s);
            }
        }

        GUILayout.Space(10f);

        if(GUILayout.Button("Set Normal Camp"))
        {
            current.SetNormalScale();
        }
    }
}
