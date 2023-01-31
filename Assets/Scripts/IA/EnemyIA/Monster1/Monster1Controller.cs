using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1Controller : EnemyController
{
    public GameObject obstacle;
    public Tile tileToPlaceObstacle;

    public override void StartEnemy()
    {
        currentEnemy.actionDone = false;
        ChangeState<Monster1CheckRange>();
    }
}
