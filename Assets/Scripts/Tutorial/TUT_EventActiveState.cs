using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_EventActiveState : BattleState
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

        while (owner.environmentEvent.playing)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.6f);

        owner.environmentEvent.StartRestartTimer();
        owner.FinishAction();
        owner.ChangeState<TutShowslideState>();

    }

    public override void Exit()
    {
        base.Exit();
        //owner.isTimeLineActive = true;
    }
}
