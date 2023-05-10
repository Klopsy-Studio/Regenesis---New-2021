using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class IsMonsterHealthWithinRange : ActionNode
{
    [SerializeField] float minimumLife;
    [SerializeField] float maxLife;

    float currentLifePercentage = 0;
    protected override void OnStart() {

        float currentHealth = owner.controller.currentEnemy.health;
        float maxHealth = owner.controller.currentEnemy.maxHealth;
        currentLifePercentage = (currentHealth / maxHealth) * 100;
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
