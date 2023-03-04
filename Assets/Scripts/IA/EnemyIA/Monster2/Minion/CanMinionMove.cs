using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CanMinionMove : ActionNode
{
    [SerializeField] MoveType moveToCheck;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {


        PlayerUnit t = new PlayerUnit();
        float value = 0f;
        Tile tileToMove = new Tile();
        List<Tile> tiles = new List<Tile>();
        List<Tile> validTiles = new List<Tile>();
        List<PlayerUnit> invalidUnits = new List<PlayerUnit>();
        MovementRange m = owner.controller.GetComponent<MovementRange>();
        m.AssignVariables(owner.controller.currentEnemy.GetComponent<MinionUnit>().movementRange);
        MonsterController controller = owner.controller;

        controller.tileToMove = null;

        switch (moveToCheck)
        {
            case MoveType.ClosestUnit:

                //Get Closest Unit
                foreach (PlayerUnit p in controller.battleController.playerUnits)
                {
                    if (p.isNearDeath)
                        continue;

                    if (Vector3.Distance(controller.currentEnemy.transform.position, p.transform.position) <= value || value == 0f)
                    {
                        t = p;
                        value = Vector3.Distance(controller.currentEnemy.transform.position, p.transform.position);
                    }
                }

                if (t == null)
                {
                    return State.Failure;
                }

                else
                {
                    List<Tile> tilesToCheck = m.GetTilesInRange(controller.battleController.board);

                    //Filter out wrong tiles

                    foreach (Tile e in tilesToCheck)
                    {
                        if (e.content == null && !e.occupied)
                        {
                            validTiles.Add(e);
                        }
                    }

                    if(validTiles == null || validTiles.Count <= 0)
                    {
                        return State.Failure;
                    }

                    float closestDistance = 0f;

                    foreach (Tile tile in validTiles)
                    {
                        if (closestDistance == 0 || Vector3.Distance(tile.transform.position, t.transform.position) <= closestDistance)
                        {
                            controller.tileToMove = tile;
                            closestDistance = Vector3.Distance(tile.transform.position, t.transform.position);
                        }
                    }

                    return State.Success;
                }

            case MoveType.LeastHealthUnit:

                foreach (PlayerUnit p in controller.battleController.playerUnits)
                {

                    if (p.isNearDeath)
                        continue;


                    if (p.health < value || value == 0)
                    {
                        t = p;
                        value = t.health;
                    }
                }

                if (t == null)
                {
                    return State.Failure;
                }

                else
                {
                    List<Tile> surroundings = t.GetSurroundings(controller.battleController.board);

                    foreach (Tile e in surroundings)
                    {
                        if (e.CheckSurroundings(controller.battleController.board) != null)
                        {
                            validTiles.Add(e);
                        }
                    }

                    controller.tileToMove = validTiles[Random.Range(0, validTiles.Count)];

                    controller.target = t;
                    return State.Success;
                }

           

            case MoveType.Random:
                List<Tile> allTiles = new List<Tile>(controller.battleController.board.playableTiles.Values);

                foreach (Tile e in allTiles)
                {
                    if (e.content == null && !e.occupied)
                    {
                        validTiles.Add(e);
                    }
                }
                if (validTiles.Count <= 0)
                {
                    return State.Failure;
                }
                else
                {
                    controller.tileToMove = validTiles[Random.Range(0, validTiles.Count)];
                    return State.Success;
                }
            default:
                return State.Success;
        }
    }
}
