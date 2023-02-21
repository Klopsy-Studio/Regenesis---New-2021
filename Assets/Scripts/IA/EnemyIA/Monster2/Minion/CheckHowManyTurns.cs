using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckHowManyTurns : ActionNode
{
    [SerializeField] ComparisonType typeOfCheck;
    [SerializeField] int numberOfTurns;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        switch (typeOfCheck)
        {
            case ComparisonType.Equals:
                if(numberOfTurns == owner.controller.turnsAlive)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.Greater:
                if (numberOfTurns > owner.controller.turnsAlive)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.GreaterAndEqual:
                if (numberOfTurns >= owner.controller.turnsAlive)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.Lower:
                if (numberOfTurns < owner.controller.turnsAlive)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.LowerAndEqual:
                if (numberOfTurns <= owner.controller.turnsAlive)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            default:
                return State.Success;
        }
    }
}
