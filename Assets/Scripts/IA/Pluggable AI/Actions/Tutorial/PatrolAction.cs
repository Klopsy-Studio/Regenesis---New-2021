using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(MonsterController controller)
    {
        Patrol(controller);
    }

    private void Patrol(MonsterController controller)
    {
        //controller.navmeshagent etc
    }
}
