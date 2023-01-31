using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterEventState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Bruh");
        owner.isTimeLineActive = false;
        StartCoroutine(ApplyEvent());
    }


    IEnumerator ApplyEvent()
    {
        owner.currentHunterEvent.Apply(owner);

        while (owner.currentHunterEvent.playing)
        {
            yield return null;
        }

        owner.currentHunterEvent.elementEnabled = false;
        owner.timelineElements.Remove(owner.currentHunterEvent);
        owner.currentHunterEvent = null;
        owner.ChangeState<TimeLineState>();
    }
}
