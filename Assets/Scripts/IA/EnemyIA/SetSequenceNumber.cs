using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetSequenceNumber : ActionNode
{
    [SerializeField] int sequenceToSet;
    protected override void OnStart() {

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
        owner.controller.lastSequence = sequenceToSet;
        return State.Success;
    }
}
