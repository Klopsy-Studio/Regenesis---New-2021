using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Move());
    }

    protected virtual IEnumerator Move()
    {
        yield return null;
    }

}
