using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TheKiwiCoder;

[CustomEditor(typeof(BehaviourTree))]
public class BehaviourTreeEditor : Editor
{

    public BehaviourTree current
    {
        get
        {
            return (BehaviourTree)target;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Assign Owner"))
        {
            current.AssignOwnerOnNodes();
        }

        if (GUILayout.Button("Activate Node Gizmos"))
        {
            current.ActivateAllGizmos();
        }

        if (GUILayout.Button("Deactivate Node Gizmos"))
        {
            current.DeactivateAllGizmos();
        }
    }
}
