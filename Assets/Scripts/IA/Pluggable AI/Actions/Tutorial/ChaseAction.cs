using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    public override void Act(MonsterController controller)
    {
        Chase(controller);
    }

    void Chase (MonsterController controller)
    {
        //controller.navmeshagent
    }

   
}
