using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MonsterHasDoneFirstTurn : ActionNode
{
    protected override void OnStart() {
        owner.controller.hasDoneFirstTurn = true;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
