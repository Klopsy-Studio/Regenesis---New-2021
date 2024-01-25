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

        owner.StartAction();
        StartCoroutine(ApplyEvent());
    }


    IEnumerator ApplyEvent()
    {
        if(owner.currentEntity != CurrentEntityTurn.Hunter)
        {
            StartCoroutine(owner.IndicateTurn(owner.turnStatusUI.playerTurn));
            while (owner.indicatingTurn)
            {
                yield return null;
            }
        }

        owner.currentEntity = CurrentEntityTurn.Hunter;
        owner.currentHunterEvent.Apply(owner);

        while (owner.currentHunterEvent.playing)
        {
            yield return null;
        }

        owner.currentHunterEvent.elementEnabled = false;
        owner.timelineElements.Remove(owner.currentHunterEvent);
        owner.currentHunterEvent = null;
        owner.FinishAction();

        owner.timelineUI.HideIconActing();

        yield return new WaitForSeconds(0.5f);
        owner.FinishAction();

        owner.ChangeState<TimeLineState>();
    }
}
