using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/FinishTurn")]
public class FinishTurnDecision : Decision
{
    public override bool Decide(MonsterController controller)
    {

        return isTurnFinished(controller);
    }

    bool isTurnFinished(MonsterController controller)
    {
        return controller.currentEnemy.actionDone;
    }
}
