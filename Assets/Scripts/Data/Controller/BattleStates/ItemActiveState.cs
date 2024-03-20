using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActiveState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.StartAction();
        owner.isTimeLineActive = false;
        StartCoroutine(ItemCoroutine());
    }

    IEnumerator ItemCoroutine()
    {
        owner.isTimeLineActive = false;

        if (owner.currentEntity != CurrentEntityTurn.Event)
        {
            StartCoroutine(owner.IndicateTurn(owner.turnStatusUI.eventTurn));
            while (owner.indicatingTurn)
            {
                yield return null;
            }
        }

        owner.currentEntity = CurrentEntityTurn.Event;

        owner.currentItem.Apply(owner);

        yield return new WaitForSeconds(2);
        owner.FinishAction();

        owner.ChangeState<TimeLineState>();
    }

    public override void Exit()
    {
        base.Exit();
        owner.isTimeLineActive = true;
    }
}
