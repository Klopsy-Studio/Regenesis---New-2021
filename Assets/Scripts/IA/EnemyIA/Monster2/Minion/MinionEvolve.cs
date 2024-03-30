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

        if (!owner.controller.hasEvolved)
        {
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

            ActionEffect.instance.Play(3, 1.5f, 0.01f, 0.05f);

            owner.controller.monsterAnimations.SetFloat("evolve", 1f);
            owner.controller.monsterAnimations.SetTrigger("evolveTrigger");
            owner.controller.currentEnemy.description.header = owner.controller.currentEnemy.GetComponent<MinionUnit>().evolvedHeader;
            owner.controller.currentEnemy.unitName = owner.controller.currentEnemy.GetComponent<MinionUnit>().evolvedName;
            owner.controller.currentEnemy.description.content = owner.controller.currentEnemy.GetComponent<MinionUnit>().evolvedDescription;
        }

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
        if (!owner.controller.hasEvolved)
        {
            evolveTime -= Time.deltaTime;

            if (evolveTime <= 0)
            {
                owner.controller.currentEnemy.AddBuff(new Modifier { modifierType = TypeOfModifier.MinionEvolve });
                owner.controller.hasEvolved = true;

                return State.Success;
            }
            else
            {
                return State.Running;
            }
        }
        else
        {
            return State.Success;
        }
    }
}
