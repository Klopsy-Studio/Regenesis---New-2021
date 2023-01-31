using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1MoveToClosestUnit : Monster1State
{
    public Tile closestTile;
    public List<Tile> tiles;
    public override void Enter()
    {
        stateName = "Move";
        base.Enter();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        //Move To Closest Player Unit
        float closestDistance = 0f;
        Unit t = new Unit();

        //T is the closest unit to the enemy
        foreach(PlayerUnit p in battleController.playerUnits)
        {
            if(Vector3.Distance(currentEnemy.transform.position, p.transform.position) <= closestDistance || closestDistance == 0f)
            {
                t = p;
                closestDistance = Vector3.Distance(currentEnemy.transform.position, p.transform.position);
            }
        }

        Movement range = owner.currentEnemy.GetComponent<Movement>();
        range.range = 3;

        //MovementRange range = GetRange<MovementRange>();
        
        //range.unit = owner.currentEnemy;
        //range.range = 3;
        //range.tile = owner.currentEnemy.tile;

        tiles = range.GetTilesInRangeForEnemy(battleController.board, false);

        List<Tile> validTiles = new List<Tile>();

        closestDistance = 0f;

        foreach (Tile tile in tiles)
        {
            if(tile.CheckSurroundings(owner.battleController.board) != null)
            {
                if (Vector3.Distance(tile.transform.position, t.tile.transform.position) <= closestDistance || closestDistance == 0f && tile.content == null)
                {
                    closestDistance = Vector3.Distance(tile.transform.position, t.tile.transform.position);
                    closestTile = tile;
                }
            }
            
        }

        Movement m = currentEnemy.GetComponent<Movement>();

        List<Tile> test = new List<Tile>();
        test.Add(closestTile);

        battleController.board.SelectMovementTiles(test);

        StartCoroutine(m.SimpleTraverse(closestTile));
        
        owner.currentEnemy.UpdateMonsterSpace(owner.battleController.board);
        battleController.board.DeSelectDefaultTiles(test);

        yield return null;

        owner.ChangeState<Monster1CheckNextAction>();
    }


    public override void Exit()
    {
        base.Exit();
        
        owner.lastAction = EnemyActions.Move;
    }
}
