using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckIfMinionHasEvolved : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (owner.controller.hasEvolved)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;

        }
    }
}
