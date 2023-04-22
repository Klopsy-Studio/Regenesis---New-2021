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
                if(owner.controller.obstaclesInGame.Count == numberOfObstacles)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }

            case ComparisonType.Greater:
                if (owner.controller.obstaclesInGame.Count > numberOfObstacles)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.GreaterAndEqual:
                if (owner.controller.obstaclesInGame.Count >= numberOfObstacles)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.Lower:
                if (owner.controller.obstaclesInGame.Count < numberOfObstacles  )
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.LowerAndEqual:
                if (owner.controller.obstaclesInGame.Count <= numberOfObstacles)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            default:
                break;
        }
        return State.Success;
    }
}
