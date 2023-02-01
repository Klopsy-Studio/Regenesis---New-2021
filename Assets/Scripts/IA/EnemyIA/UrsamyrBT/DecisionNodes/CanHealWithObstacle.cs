using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CanHealWithObstacle : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (owner.controller.validObstacles.Count > 0 && owner.controller.currentEnemy.health < owner.controller.currentEnemy.maxHealth)
        {
            return State.Success;
        }

        else
        {
            return State.Failure;
        }
    }
}
