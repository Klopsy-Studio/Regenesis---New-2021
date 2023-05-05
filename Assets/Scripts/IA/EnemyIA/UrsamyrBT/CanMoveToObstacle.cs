using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CanMoveToObstacle : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
        //Get Farthest Obstacle
        List<BearObstacleScript> validObstacles = new List<BearObstacleScript>();
        float maxDistance = 0;
        BearObstacleScript obstacleTarget = new BearObstacleScript();

        foreach(BearObstacleScript o in owner.controller.obstaclesInGame)
        {
            if (o.IsObstacleValid(owner.controller.battleController.board))
            {
                if(Vector3.Distance(owner.controller.transform.position, o.transform.position) >= maxDistance)
                {
                    maxDistance = Vector3.Distance(owner.controller.transform.position, o.transform.position);
                    obstacleTarget = o;
                }
            }
        }

        if(maxDistance == 0)
        {
            return State.Failure;
        }

        maxDistance = 0;
        List<Tile> obstacleTiles = obstacleTarget.GetValidTiles(owner.controller.battleController.board);
        owner.controller.chosenObstacle = obstacleTarget;
        foreach(Tile t in obstacleTiles)
        {
            if(Vector3.Distance(owner.controller.transform.position, t.transform.position) >= maxDistance)
            {
                maxDistance = Vector3.Distance(owner.controller.transform.position, t.transform.position);
                owner.controller.tileToMove = t;
            }
        }

        return State.Success;
    }
}
