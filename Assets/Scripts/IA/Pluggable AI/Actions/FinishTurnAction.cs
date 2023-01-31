using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableAI/Actions/FinishTurnAction")]
public class FinishTurnAction : Action
{
    public override void Act(MonsterController controller)
    {
        controller.battleController.ChangeState<FinishEnemyUnitTurnState>();
        
    }
}
