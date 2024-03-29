using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActiveState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.StartAction();
        owner.isTimeLineActive = false;
        owner.turnStatusUI.ActivateTurn("Event");
        StartCoroutine(EventCoroutine());
    }

    IEnumerator EventCoroutine()
    {
        owner.isTimeLineActive = false;

        if(owner.currentEntity != CurrentEntityTurn.Event)
        {
            StartCoroutine(owner.IndicateTurn(owner.turnStatusUI.eventTurn));
            while (owner.indicatingTurn)
            {
                yield return null;
            }
        }

        owner.currentEntity = CurrentEntityTurn.Event;

        owner.environmentEvent.ApplyEffect();

        while (owner.environmentEvent.playing)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.6f);

        owner.timelineUI.HideIconActing();

        yield return new WaitForSeconds(0.5f);
        owner.environmentEvent.StartRestartTimer();
        owner.FinishAction();
        owner.ChangeState<TimeLineState>();

    }

    public override void Exit()
    {
        base.Exit();
        owner.isTimeLineActive = true;
    }
}
