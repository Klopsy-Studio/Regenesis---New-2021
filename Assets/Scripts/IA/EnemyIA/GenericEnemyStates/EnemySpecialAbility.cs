using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialAbility : EnemyState
{
    public override void Enter()
    {
        base.Enter();
    }

    protected virtual IEnumerator SpecialAbility()
    {
        yield return null;
    }
}
