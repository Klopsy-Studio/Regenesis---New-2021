using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[System.Serializable]
[CreateAssetMenu(menuName = "PluggableAI/Decisions/Check Attack Range Decision")]
public class CheckAttackRangeDecision : Decision
{
    public List<MonsterAbility> monsterAbilities;

    public override bool Decide(MonsterController controller)
    {
        return CheckAttackRange(controller);
    }

    
    bool CheckAttackRange(MonsterController controller)
    {
        foreach(MonsterAbility ability in monsterAbilities)
        {
            if (ability.CheckIfAttackIsValid(controller))
            {
                controller.validAbilities.Add(ability);
                return true;
            }
        }

        return false;
    }
}




