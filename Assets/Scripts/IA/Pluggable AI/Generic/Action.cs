using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public abstract void Act(MonsterController controller);
    protected virtual void OnExit(MonsterController controller)
    {
        controller.currentState.CheckTransitions(controller);
        controller.currentState.UpdateState(controller);
    }
}
