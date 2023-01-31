using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomMovement : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(RandomMovement());
    }

    IEnumerator RandomMovement()
    {
        //Movement m = currentEnemy.GetComponent<Movement>();

        //List<Tile> randomTiles = GetTiles(currentEnemy.randomRange);

        //StartCoroutine(m.Traverse(randomTiles.ToArray()[Random.Range(0, randomTiles.ToArray().Length)]));

        //while (m.moving)
        //{
        //    yield return null;
        //}

        //battleController.ChangeState<FinishEnemyUnitTurnState>();
        yield return null;
    }
}
