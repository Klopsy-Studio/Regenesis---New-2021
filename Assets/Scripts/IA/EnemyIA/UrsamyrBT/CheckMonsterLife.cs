using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckMonsterLife : ActionNode
{
    [SerializeField] float lifePercentage;

    [SerializeField] ComparisonType comparison;


    float currentLifePercentage;
    protected override void OnStart() {

        currentLifePercentage = (owner.controller.currentEnemy.health / owner.controller.currentEnemy.maxHealth) * 100;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        switch (comparison)
        {
            case ComparisonType.Equals:
                if(currentLifePercentage == lifePercentage)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.Greater:
                if (currentLifePercentage > lifePercentage)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.GreaterAndEqual:
                if (currentLifePercentage >= lifePercentage)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.Lower:
                if (currentLifePercentage < lifePercentage)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.LowerAndEqual:
                if (currentLifePercentage <= lifePercentage)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            default:
                return State.Failure;
        }

    }
}
