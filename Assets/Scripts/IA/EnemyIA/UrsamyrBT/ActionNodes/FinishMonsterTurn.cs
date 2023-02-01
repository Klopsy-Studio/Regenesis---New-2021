using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class FinishMonsterTurn : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        owner.controller.test = false;
        owner.controller.battleController.ChangeState<FinishEnemyUnitTurnState>();
        return State.Success;    
    }
}
