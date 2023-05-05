using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class FinishMonsterTurn : ActionNode
{
    [SerializeField] TimelineVelocity finishVelocity = TimelineVelocity.Normal;
    protected override void OnStart() {
        owner.controller.test = false;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        owner.controller.currentEnemy.timelineVelocity = finishVelocity;
        owner.controller.turnFinished = true;
        owner.controller.currentEnemy.SetCurrentVelocity();
        owner.controller.battleController.ChangeState<FinishEnemyUnitTurnState>();
        return State.Success;    
    }
}
