using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_FinishEnemyUnitTurnState : BattleState
{
    public override void Enter()
    {
        Debug.Log("HA ENTRADO A TUT FINISHENEMY UNIT TURN STATE");
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

    }

    IEnumerator FinishTurnCoroutine()
    {
        Debug.Log("Ending enemy turn");
        owner.currentEnemyUnit.timelineFill = 0;
        owner.timelineUI.ShowTimelineIcon(owner.currentEnemyUnit);

        yield return null;
        //owner.currentEnemyUnit.monsterControl.tree = null;
        owner.currentEnemyUnit = null;
        owner.currentEnemyController = null;
        owner.ChangeState<TUT_TimelineStateFirstHunter>();
    }

}
