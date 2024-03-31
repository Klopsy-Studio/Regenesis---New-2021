using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "ScriptableObjects/TutorialDialogue", order = 1)]
public class DialogueText : ScriptableObject
{
    public Line[] dialogueLines;
}

[Serializable]
public class Line
{
    public Sprite portrait;
    public string line;
    public string speakerSound;
}
