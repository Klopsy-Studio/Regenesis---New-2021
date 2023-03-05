using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class HasMonsterSpawnedLastTurn : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (owner.controller.hasSpawnedMinionsInLastTurn)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
