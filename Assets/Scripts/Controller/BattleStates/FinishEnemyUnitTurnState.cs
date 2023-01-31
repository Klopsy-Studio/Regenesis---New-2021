using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishEnemyUnitTurnState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.monsterController.currentEnemy.GetComponent<Movement>().isTraverseCalled = false;
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

        owner.currentEnemyUnit = null;
        owner.currentEnemyController = null;
        owner.ChangeState<TimeLineState>();
    }
}
