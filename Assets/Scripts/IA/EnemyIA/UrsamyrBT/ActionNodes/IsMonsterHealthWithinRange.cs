using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class IsMonsterHealthWithinRange : ActionNode
{
    [SerializeField] float minimumLife;
    [SerializeField] float maxLife;

    float currentLifePercentage;
    protected override void OnStart() {

        currentLifePercentage = (owner.controller.currentEnemy.health / owner.controller.currentEnemy.maxHealth) * 100;
        Debug.Log("Percentage = " + currentLifePercentage + "%");
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if(currentLifePercentage >= minimumLife && currentLifePercentage <= maxLife)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
