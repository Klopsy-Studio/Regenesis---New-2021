using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Dialogue/New Conversation")]

public class Conversation : ScriptableObject
{
    public Dialogue[] dialogue;
}


[System.Serializable]
public class Dialogue
{
    //0 kaeo
    //1 isak
    //2 ola
    [Tooltip("0 is for Kaeo, 1 is for Isak, 2 is for Ola")]
    public int user;

    [TextArea]
    public string line;
}
