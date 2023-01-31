using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitDeathState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.isTimeLineActive = false;
        StartCoroutine(DeathSequence());
    }


    IEnumerator DeathSequence()
    {
        SelectTile(owner.currentUnit.currentPoint);
        yield return new WaitForSeconds(2f);
        owner.currentUnit.deathElement.DisableDeath(owner);

        owner.currentUnit.Die();

        yield return null;
        
        owner.isTimeLineActive = true;
        owner.ChangeState<TimeLineState>();
    }
}
