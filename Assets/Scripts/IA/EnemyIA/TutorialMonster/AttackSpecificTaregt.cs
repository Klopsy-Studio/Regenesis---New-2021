using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class AttackSpecificTaregt : ActionNode
{
    [SerializeField] int unitToAttack;
    protected override void OnStart() {

        owner.controller.target = owner.controller.battleController.playerUnits[unitToAttack].GetComponent<PlayerUnit>();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
