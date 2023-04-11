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
