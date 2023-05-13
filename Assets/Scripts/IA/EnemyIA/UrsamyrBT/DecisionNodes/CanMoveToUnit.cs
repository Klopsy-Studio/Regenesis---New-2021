using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CanMoveToUnit : ActionNode
{
    [SerializeField] MoveType moveToCheck;
    [SerializeField] WhichMonster monsterToCheck = WhichMonster.BearMonster;
    [SerializeField] int specificUnit = 0;
    [SerializeField] Point specificTile;
    [SerializeField] bool differentUnit;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    //Check if unit is valid to move to
    public bool CheckIfUnitIsValid(PlayerUnit unit, BattleController controller)
    {
        List<Tile> surroundingTiles = unit.GetSurroundings(controller.board);

        foreach (Tile s in surroundingTiles)
        {
            if (s.CheckSurroundings(controller.board) != null)
            {
                return true;
            }
        }

        return false;
    }
    protected override State OnUpdate() {
           if (owner.controller.turnFinished)
            return State.Success;
        
        PlayerUnit t = new PlayerUnit();
        float value = 0f;
        Tile tileToMove = new Tile();
        List<Tile> tiles = new List<Tile>();
        List<Tile> validTiles = new List<Tile>();
        List<PlayerUnit> invalidUnits = new List<PlayerUnit>();
        Movement m = owner.controller.GetComponent<Movement>();
        MonsterController controller = owner.controller;


        controller.tileToMove = null;

        switch (moveToCheck)
        {
            case MoveType.ClosestUnit:

                switch (monsterToCheck)
                {
                    case WhichMonster.BearMonster:

                        foreach (PlayerUnit p in controller.battleController.playerUnits)
                        {
                            if (p.isNearDeath)
                                continue;

                            if (!CheckIfUnitIsValid(p, controller.battleController))
                            {
                                continue;
                            }

                            if (Vector3.Distance(controller.currentEnemy.transform.position, p.transform.position) <= value || value == 0f)
                            {
                                if (differentUnit)
                                {
                                    if(owner.controller.previousTarget != null)
                                    {
                                        if (owner.controller.previousTarget != p)
                                        {
                                            t = p;
                                            value = Vector3.Distance(controller.currentEnemy.transform.position, p.transform.position);
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    t = p;
                                    value = Vector3.Distance(controller.currentEnemy.transform.position, p.transform.position);
                                }
                                
                                
                            }
                        }

                        if (t == null)
                        {
                            return State.Failure;
                        }

                        else
                        {
                            owner.controller.previousTarget = t;
                            
                            List<Tile> surroundings = t.GetSurroundings(controller.battleController.board);

                            foreach (Tile e in surroundings)
                            {
                                if (e.CheckSurroundings(controller.battleController.board) != null && e != owner.controller.currentEnemy.tile)
                                {
                                    validTiles.Add(e);
                                }
                            }
                            

                            if(validTiles.Count > 0)
                            {
                                controller.tileToMove = validTiles[Random.Range(0, validTiles.Count)];
                                controller.target = t;
                                return State.Success;
                            }
                            else
                            {

                                Debug.Log("No valid tiles avaible");
                                return State.Failure;

                            }
                        }
                    case WhichMonster.SpiderMonster:

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
                            List<Tile> trashTiles = new List<Tile>();

                            foreach(RangeData d in owner.controller.spiderMonsterMovementRange)
                            {
                                AbilityRange a = d.GetOrCreateRange(d.range, owner.controller.gameObject);
                                a.unit = owner.controller.currentEnemy;
                                
                                trashTiles = a.GetTilesInRange(owner.controller.battleController.board);

                                foreach (Tile e in trashTiles)
                                {
                                    if (e.CheckSurroundings(controller.battleController.board) != null && e.content == null && e != owner.controller.currentEnemy.tile)
                                    {
                                        validTiles.Add(e);
                                    }
                                }
                            }

                            float closestDistanceTiles = 0f;
                            foreach (Tile e in validTiles)
                            {
                                if (closestDistanceTiles == 0 || Vector3.Distance(e.transform.position, t.transform.position) <= closestDistanceTiles)
                                {
                                    closestDistanceTiles = Vector3.Distance(e.transform.position, t.transform.position);
                                    controller.tileToMove = e;
                                }
                            }

                            controller.target = t;
                            return State.Success;
                        }
                    default:
                        return State.Success;
                }

                

            case MoveType.LeastHealthUnit:

                foreach (PlayerUnit p in controller.battleController.playerUnits)
                {

                    if (p.isNearDeath)
                        continue;

                    if(!CheckIfUnitIsValid(p, controller.battleController))
                    {
                        continue;
                    }

                    
                    if (p.health < value || value == 0)
                    {
                        if (differentUnit)
                        {
                            if(owner.controller.previousTarget != null)
                            {
                                if(owner.controller.previousTarget != p)
                                {
                                    t = p;
                                    value = t.health;
                                }
                            }
                        }
                        else
                        {
                            t = p;
                            value = t.health;
                        }

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
                        if (e.CheckSurroundings(controller.battleController.board) != null && e != owner.controller.currentEnemy.tile)
                        {
                            validTiles.Add(e);
                        }
                    }

                    if(validTiles.Count > 0)
                    {
                        controller.tileToMove = validTiles[Random.Range(0, validTiles.Count)];

                        controller.target = t;
                        return State.Success;
                    }
                    else
                    {
                        return State.Failure;
                    }
                   
                }

            case MoveType.GetAway:
                m.range = 8;

                tiles = m.GetTilesInRangeForEnemy(controller.battleController.board, false);

                foreach (Tile tile in tiles)
                {
                    if (tile.CheckSurroundings(controller.battleController.board) != null)
                    {
                        validTiles.Add(tile);
                    }
                }

                if (validTiles.Count == 0)
                {
                    return State.Failure;
                }
                else
                {
                    foreach (Tile tile in tiles)
                    {
                        if (value == 0 || Vector3.Distance(controller.currentEnemy.transform.position, tile.transform.position) >= value)
                        {
                            controller.tileToMove = tile;
                            value = Vector3.Distance(controller.currentEnemy.transform.position, tile.transform.position);
                        }
                    }

                    return State.Success;
                }

            case MoveType.Random:
                List<Tile> allTiles = new List<Tile>(controller.battleController.board.playableTiles.Values);

                foreach (Tile e in allTiles)
                {
                    if (e.CheckSurroundings(controller.battleController.board) != null)
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

            case MoveType.SpecificUnit:

                PlayerUnit h = controller.battleController.playerUnits[specificUnit].GetComponent<PlayerUnit>();

                if (!CheckIfUnitIsValid(h, controller.battleController))
                {
                    return State.Failure;
                }


                owner.controller.previousTarget = h;

                List<Tile> sur = h.GetSurroundings(controller.battleController.board);

                foreach (Tile e in sur)
                {
                    if (e.CheckSurroundings(controller.battleController.board) != null && e != owner.controller.currentEnemy.tile)
                    {
                        validTiles.Add(e);
                    }
                }

                if (validTiles.Count > 0)
                {
                    controller.tileToMove = validTiles[Random.Range(0, validTiles.Count)];
                    controller.target = h;
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }


            case MoveType.SpecificTile:

                Tile ñ = owner.controller.battleController.board.GetTile(specificTile);

                if(ñ != null)
                {
                    if (ñ.CheckSurroundings(controller.battleController.board) != null && ñ != owner.controller.currentEnemy.tile)
                    {
                        owner.controller.tileToMove = ñ;
                        return State.Success;
                    }
                    else
                    {
                        return State.Failure;
                    }
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

[System.Serializable]
enum WhichMonster
{
    BearMonster, SpiderMonster
}
