using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CanMoveCloseToUnit : ActionNode
{
    [SerializeField] RangeData unitRange;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
        PlayerUnit hunter = owner.controller.GetClosestHunter();
        owner.controller.tileToMove = hunter.GetNearestAvaibleTile(unitRange);
        return State.Success;
    }
}
