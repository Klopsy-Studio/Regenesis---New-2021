using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MonsterFirstTurnCheck : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
        if (!owner.controller.hasDoneFirstTurn)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
