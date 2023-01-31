using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapons))]
public class WeaponsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10f);

        if (GUILayout.Button("Set Weapons's Default Values"))
        {
            current.SetDefaultValues();
        }
    }


    public Weapons current
    {
        get
        {
            return (Weapons)target;
        }
    }
}
