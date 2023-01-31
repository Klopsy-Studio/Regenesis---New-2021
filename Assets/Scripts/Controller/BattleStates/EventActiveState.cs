using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActiveState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.isTimeLineActive = false;
        owner.turnStatusUI.ActivateTurn("Event");
        StartCoroutine(EventCoroutine());
    }

    IEnumerator EventCoroutine()
    {
        owner.isTimeLineActive = false;
        owner.turnStatusUI.IndicateTurnStatus(owner.turnStatusUI.eventTurn);
        yield return new WaitForSeconds(1f);
        owner.turnStatusUI.StopTurnStatus();
        yield return new WaitForSeconds(1f);

        owner.environmentEvent.ApplyEffect();
        yield return new WaitForSeconds(1);
        owner.ChangeState<TimeLineState>();

    }

    public override void Exit()
    {
        base.Exit();
        owner.isTimeLineActive = true;
    }
}
