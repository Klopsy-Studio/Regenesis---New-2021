using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActiveState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.isTimeLineActive = false;
        StartCoroutine(ItemCoroutine());
    }

    IEnumerator ItemCoroutine()
    {
        owner.isTimeLineActive = false;
        owner.turnStatusUI.IndicateTurnStatus(owner.turnStatusUI.eventTurn);
        yield return new WaitForSeconds(1f);
        owner.turnStatusUI.StopTurnStatus();
        yield return new WaitForSeconds(1f);

        owner.currentItem.Apply(owner);

        yield return new WaitForSeconds(2);
        owner.ChangeState<TimeLineState>();
    }

    public override void Exit()
    {
        base.Exit();
        owner.isTimeLineActive = true;
    }
}
