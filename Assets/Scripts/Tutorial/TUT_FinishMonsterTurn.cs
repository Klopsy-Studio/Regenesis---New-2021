using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class TUT_FinishMonsterTurn : ActionNode
{
    [SerializeField] TimelineVelocity finishVelocity = TimelineVelocity.Normal;
    protected override void OnStart()
    {
        owner.controller.test = false;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        owner.controller.currentEnemy.timelineVelocity = finishVelocity;
        owner.controller.currentEnemy.SetCurrentVelocity();
        owner.controller.battleController.ChangeState<TUT_FinishEnemyUnitTurnState>();
    
        //owner.controller.battleController.ChangeState<>
        return State.Success;
    }
}
