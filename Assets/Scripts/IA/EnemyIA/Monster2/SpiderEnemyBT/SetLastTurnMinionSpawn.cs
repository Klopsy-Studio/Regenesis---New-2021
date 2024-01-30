using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetLastTurnMinionSpawn : ActionNode
{
    [SerializeField] bool value;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
        //owner.controller.hasSpawnedMinionsInLastTurn = value;
        return State.Success;
    }
}
