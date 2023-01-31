using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextUnitState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(SetUnit());
    }

    IEnumerator SetUnit()
    {
        if(owner.currentUnit.turnEnded)
        {
            unitsWithActions.Remove(owner.currentUnit);     
        }
        owner.currentUnit = null;

        yield return null;
        owner.ChangeState<SelectUnitState>();
    }
}
