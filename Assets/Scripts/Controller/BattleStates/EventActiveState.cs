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

        StartCoroutine(owner.IndicateTurn(owner.turnStatusUI.eventTurn));
        while (owner.indicatingTurn)
        {
            yield return null;
        }

        owner.environmentEvent.ApplyEffect();
        yield return new WaitForSeconds(1);

        owner.FinishAction();
        owner.ChangeState<TimeLineState>();

    }

    public override void Exit()
    {
        base.Exit();
        owner.isTimeLineActive = true;
    }
}
