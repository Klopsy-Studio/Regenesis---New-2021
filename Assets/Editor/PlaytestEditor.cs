using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Playtest))]
public class PlaytestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10f);

        if(current.elements != null)
        {
            foreach (TimelineElements e in current.elements)
            {
                if (GUILayout.Button("Set " + e.name + "'s turn"))
                {
                    current.JumpToUnitTurn(e);
                }
            }
        }

        GUILayout.Space(10f);
        if (GUILayout.Button("Play Action Effect"))
        {
            current.PlayActionEffect();
        }

    }


    public Playtest current
    {
        get
        {
            return (Playtest)target;
        }
    }
}
