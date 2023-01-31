using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/ActiveState")]
public class ActivateStateDecision : Decision
{
    public override bool Decide(MonsterController controller)
    {
        bool chaseTarget = true;
        return chaseTarget;
    }

   
}
