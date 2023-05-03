using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckIfUnitsInRange : ActionNode
{
    [SerializeField] List<MonsterAbility> abilities;

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }
    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
        if (abilities != null)
        {
            if(abilities.Count > 0)
            {
                foreach (MonsterAbility ability in abilities)
                {
                    if (ability.CheckIfAttackIsValid(owner.controller))
                    {
                        owner.controller.validAbilities.Add(ability);
                        return State.Success;
                    }
                }
            }
        }
        
        return State.Failure;
    }
}
