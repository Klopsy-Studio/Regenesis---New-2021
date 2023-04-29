using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class AnimationEventCopier : EditorWindow
{
    public AnimationClip[] sourceClips;
    public AnimationClip[] targetClips;

    private AnimationEvent[] clearEvents;

    private SerializedObject so;

    [MenuItem("Window/Animation Event Copier")]

    static void Init()
    {
        GetWindow(typeof(AnimationEventCopier));
    }
    void OnGUI()
    {
        CreateDisplayList("sourceClips");
        CreateDisplayList("targetClips");

        if (sourceClips.Length > 0 && targetClips.Length > 0)
        {
            if (sourceClips.Length == targetClips.Length)
            {
                if (IsMyListValid(sourceClips) && IsMyListValid(targetClips))
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Copy"))
                        CopyData();
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

    }

    void CopyData()
    {
        for (int i = 0; i < sourceClips.Length; i++)
        {
            Undo.RegisterCompleteObjectUndo(targetClips[i], "Undo Generic Copy");

            AnimationClip sourceAnimClip = sourceClips[i] as AnimationClip;
            AnimationClip targetAnimClip = targetClips[i] as AnimationClip;

            if (sourceAnimClip != targetAnimClip)
            {
                AnimationUtility.SetAnimationEvents(targetAnimClip, clearEvents);
                AnimationUtility.SetAnimationEvents(targetAnimClip, AnimationUtility.GetAnimationEvents(sourceAnimClip));
            }
        }
        
    }

    public void CreateDisplayList(string property)
    {
        EditorGUILayout.BeginVertical();
        ScriptableObject target = this;
        if (so == null)
        {
            so = new SerializedObject(target);
        }
        SerializedProperty stringsProperty = so.FindProperty(property);
        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();
        EditorGUILayout.EndVertical();
    }

    bool IsMyListValid(AnimationClip[] target)
    {
        for (int i = 0; i < target.Length; i++)
        {
            if (target[i] == null)
            {
                return false;
            }
        }

        return true;
    }
}


