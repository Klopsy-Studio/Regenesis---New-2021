using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Can Heal")]

public class CanHealDecision : Decision
{
    public override bool Decide(MonsterController controller)
    {
        if(controller.validObstacles.Count > 0 && controller.currentEnemy.health < controller.currentEnemy.maxHealth)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
