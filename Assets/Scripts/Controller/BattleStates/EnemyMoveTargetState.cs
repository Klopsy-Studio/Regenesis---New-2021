using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveTargetState : BattleState
{
    Point[] targetDir = new Point[4]
    {
        new Point(0, 1),
        new Point(0, -1),
        new Point(1, 0),
        new Point(-1, 0)
    };


    List<Tile> tiles;
 


    float minDist = float.PositiveInfinity;
    float nearestCell = float.PositiveInfinity;
    Unit targetUnit;
    //public override void Enter()
    //{
    //    base.Enter();
    //    owner.isTimeLineActive = false;
    //    Movement mover = owner.currentEnemyUnit.GetComponent<Movement>();
    //    tiles = mover.GetTilesInRange(board, true);

      
    //    StartCoroutine(Sequence());
    //}
    public override void Exit()
    {
      
        base.Exit();
        board.DeSelectTiles(tiles);
        tiles = null;
        minDist = float.PositiveInfinity;
        nearestCell = float.PositiveInfinity;
    }

    IEnumerator Sequence()
    {
       
        for (int i = 0; i < playerUnits.Count; i++)
        {
            Unit playerPos = playerUnits[i];
            var d = Vector3.Distance(owner.currentEnemyUnit.transform.position,playerPos.transform.position);
            if (d < minDist)
            {
                minDist = d;
                targetUnit = playerPos;
                owner.currentEnemyUnit.target = targetUnit;
            }
        }

        for (int i = 0; i < targetDir.Length; i++)
        {
            var targetPos = targetUnit.currentPoint + targetDir[i];
            if (!board.playableTiles.ContainsKey(targetPos))
            {
                continue;
            }
            Tile t = board.playableTiles[targetPos];
            var d = Vector3.Distance(t.transform.position, owner.currentEnemyUnit.transform.position);
            if (d < nearestCell)
            {
                nearestCell = d;
                SelectTile(targetPos);
            }
        }

        Movement m = owner.currentEnemyUnit.GetComponent<Movement>();

        owner.currentEnemyUnit.currentPoint = owner.currentTile.pos;
        yield return StartCoroutine(m.SimpleTraverse(owner.currentTile));
        

        while (m.moving)
        {
            yield return null;
        }
        
        owner.ChangeState<EnemyAttackState>();
    }

}
