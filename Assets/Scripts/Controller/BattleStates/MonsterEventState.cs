using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEventState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.isTimeLineActive = false;
        StartCoroutine(ApplyEvent());
    }


    IEnumerator ApplyEvent()
    {
        owner.currentMonsterEvent.Apply();

        while (owner.currentMonsterEvent.acting)
        {
            yield return null;
        }

        owner.currentMonsterEvent.elementEnabled = false;

        owner.timelineElements.Remove(owner.currentMonsterEvent);
        owner.currentMonsterEvent = null;
        owner.ChangeState<TimeLineState>();
    }
}
