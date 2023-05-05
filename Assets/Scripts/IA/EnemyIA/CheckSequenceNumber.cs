using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckSequenceNumber : ActionNode
{
    [SerializeField] int sequenceCheck;
    protected override void OnStart() {

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
        if(sequenceCheck != owner.controller.lastSequence)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
