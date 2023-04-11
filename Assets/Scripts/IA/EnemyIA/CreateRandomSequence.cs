using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CreateRandomSequence : ActionNode
{
    [SerializeField] int minimumSequenceNumber;
    [SerializeField] int maximumSequenceNumber;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        owner.controller.currentSequence = Random.Range(minimumSequenceNumber, maximumSequenceNumber);
        return State.Success;
    }
}
