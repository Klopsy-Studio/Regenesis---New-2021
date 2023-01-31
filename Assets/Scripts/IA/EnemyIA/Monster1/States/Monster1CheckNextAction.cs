using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1CheckNextAction : Monster1State
{

    public override void Enter()
    {
        stateName = "Checking Next Action";
        base.Enter();
        StartCoroutine(CheckActions());
    }

    IEnumerator CheckActions()
    {
        if (owner.currentEnemy.actionDone)
        {
            yield return null;
            battleController.ChangeState<FinishEnemyUnitTurnState>();
        }
        else
        {
            yield return null;
            owner.ChangeState<Monster1CheckRange>();
        }
    }
}
