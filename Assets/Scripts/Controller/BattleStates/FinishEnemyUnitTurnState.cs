using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class FinishEnemyUnitTurnState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.monsterController.currentEnemy.GetComponent<Movement>().isTraverseCalled = false;
        owner.FinishAction();
        owner.miniStatus.DeactivateStatus();
        owner.board.DeactivateTileSelection();
        owner.monsterController.currentState = owner.monsterController.startState;
        owner.monsterController.isUpdatingState = false;
        StartCoroutine(FinishTurnCoroutine());
    }

    public override void Exit()
    {
        base.Exit();
        owner.isTimeLineActive = true;
    }

    IEnumerator FinishTurnCoroutine()
    {
        Debug.Log("Ending enemy turn");
        owner.currentEnemyUnit.timelineFill = 0;
       
        yield return null;
        //owner.currentEnemyUnit.monsterControl.tree = null;


        owner.timelineUI.ShowTimelineIcon(owner.currentEnemyUnit);
        owner.timelineUI.HideIconActing();
        owner.currentEnemyUnit = null;
        owner.currentEnemyController = null;
        yield return new WaitForSeconds(0.5f);

        owner.ChangeState<TimeLineState>();
    }
}
