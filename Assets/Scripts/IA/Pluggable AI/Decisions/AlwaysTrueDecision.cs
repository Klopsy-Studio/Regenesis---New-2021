using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Always true decision")]

public class AlwaysTrueDecision : Decision
{
    public override bool Decide(MonsterController controller)
    {
        return true;
    }
}
