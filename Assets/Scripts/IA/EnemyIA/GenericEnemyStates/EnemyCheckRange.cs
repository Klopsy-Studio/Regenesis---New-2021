using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheckRange : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(CheckRange());
    }

    protected virtual IEnumerator CheckRange()
    {
        yield return null;
    }
}
