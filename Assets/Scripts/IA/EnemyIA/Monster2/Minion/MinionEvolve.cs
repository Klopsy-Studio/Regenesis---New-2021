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

    [SerializeField] float evolveTime = 2f;
    protected override void OnStart() {
        owner.controller.currentEnemy.power = evolvedPower;
        owner.controller.currentEnemy.criticalPercentage = evolvedCrt;

        owner.controller.currentEnemy.unitPortrait = owner.controller.currentEnemy.evolvedPortrait;
        int healthToGive = evolvedHealth - owner.controller.currentEnemy.maxHealth;
        owner.controller.currentEnemy.maxHealth = evolvedHealth;

        owner.controller.currentEnemy.health += healthToGive;

        if (owner.controller.currentEnemy.health >= owner.controller.currentEnemy.maxHealth)
        {
            owner.controller.currentEnemy.health = owner.controller.currentEnemy.maxHealth;
        }

        owner.controller.hasEvolved = true;
        ActionEffect.instance.Play(3, 1.5f, 0.01f, 0.05f);

        owner.controller.monsterAnimations.SetFloat("evolve", 1f);
        owner.controller.monsterAnimations.SetTrigger("evolveTrigger");
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        evolveTime -= Time.deltaTime;

        if(evolveTime <= 0)
        {
            return State.Success;

        }
        else
        {
            return State.Running;
        }

    }
}
