using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialDialogueCombat : TutorialDialogue
{
    [Space]
    [Header("Combat tutorial components")]
    [Space] 
    [SerializeField] UnityEvent endDialogueEvent;


    public override void Disable()
    {
        base.Disable();
        endDialogueEvent.Invoke();
    }
}
