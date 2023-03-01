using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MinionEvolve : ActionNode
{
    [SerializeField] int evolvedPower;
    [SerializeField] int evolvedCrt;
    [SerializeField] int evolvedHealth;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        owner.controller.currentEnemy.power = evolvedPower;
        owner.controller.currentEnemy.criticalPercentage = evolvedCrt;

        int healthToGive = evolvedHealth - owner.controller.currentEnemy.maxHealth;
        owner.controller.currentEnemy.maxHealth = evolvedHealth;

        owner.controller.currentEnemy.health += healthToGive;

        if(owner.controller.currentEnemy.health >= owner.controller.currentEnemy.maxHealth)
        {
            owner.controller.currentEnemy.health = owner.controller.currentEnemy.maxHealth;
        }

        owner.controller.hasEvolved = true;

        owner.controller.monsterAnimations.SetBool("evolved", true);
        owner.controller.monsterAnimations.SetBool("regular", false);
        return State.Success;
    }
}
