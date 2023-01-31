using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Can do specific Attack Decision")]

public class CanDoSpecificAttack : Decision
{
    [SerializeField] MonsterAbility specificAbility;


    public override bool Decide(MonsterController controller)
    {
        return CheckAttack(controller);
    }


    bool CheckAttack(MonsterController controller)
    {
        if (specificAbility.CheckIfAttackIsValid(controller))
        {
            controller.validAttack = specificAbility;
            return true;
        }

        else
        {
            return false;
        }
    }
}
