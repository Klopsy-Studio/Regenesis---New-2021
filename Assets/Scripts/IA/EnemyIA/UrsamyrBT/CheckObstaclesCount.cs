using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckObstaclesCount : ActionNode
{
    [SerializeField] int numberOfObstacles;
    [SerializeField] ComparisonType comparison;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        switch (comparison)
        {
            case ComparisonType.Equals:
                if(numberOfObstacles == owner.controller.obstaclesInGame.Count)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }

            case ComparisonType.Greater:
                if (numberOfObstacles > owner.controller.obstaclesInGame.Count)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.GreaterAndEqual:
                if (numberOfObstacles >= owner.controller.obstaclesInGame.Count)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.Lower:
                if (numberOfObstacles < owner.controller.obstaclesInGame.Count)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.LowerAndEqual:
                if (numberOfObstacles <= owner.controller.obstaclesInGame.Count)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
                break;
            default:
                break;
        }
        return State.Success;
    }
}
