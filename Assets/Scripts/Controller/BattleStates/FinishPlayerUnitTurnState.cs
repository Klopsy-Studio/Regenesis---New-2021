using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPlayerUnitTurnState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(FinishTurnCoroutine());
    }

    public override void Exit()
    {
        base.Exit();
        owner.isTimeLineActive = true;
    }

    IEnumerator FinishTurnCoroutine()
    {
       
        owner.currentUnit.SetVelocityWhenTurnIsFinished();
        //Debug.Log("CURRENT VELOCITY ES " + owner.currentUnit.TimelineVelocity + " CURRENT UNIT ACTIONS " + owner.currentUnit.ActionsPerTurn);
        owner.currentUnit.didNotMove = true;
        owner.currentUnit.timelineFill = 0;
        owner.currentUnit.status.ChangeToSmall();
        owner.currentUnit.playerUI.HideActionPoints();
        owner.currentUnit.iconTimeline.SetTimelineIconTextVelocity();
        owner.board.DeactivateTileSelection();
        //AudioManager.instance.Play("TurnEnd");
        yield return null;
        owner.ChangeState<TimeLineState>();
    }
}
