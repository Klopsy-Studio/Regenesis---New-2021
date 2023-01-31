using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public override bool Decide (MonsterController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    bool Look (MonsterController controller)
    {
        Debug.DrawRay(controller.transform.position, controller.transform.forward * 5, Color.green);
        ////DO SOMETHING
        //if (Physics.raycast)
        //{
        //    return true;

        //}
        //else
        //{
        //    false;
        //}

        return true;

    }
}
