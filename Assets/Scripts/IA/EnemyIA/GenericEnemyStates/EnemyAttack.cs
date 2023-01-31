using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Attack());
    }


    protected virtual IEnumerator Attack()
    {
        yield return null;
    }
}
