using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SelectObject))]
public class SelectObjectEditor : Editor
{
    bool running = false, runningOld = false;
    GameObject mySelection;

    public SelectObject current
    {
        get
        {
            return (SelectObject)target;
        }
    }

    private void OnSceneGUI()
    {
        Selection.SetActiveObjectWithContext(current.selection, current.selection);
    }

    


}
