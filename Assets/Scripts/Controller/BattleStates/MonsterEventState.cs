using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEventState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.isTimeLineActive = false;

        owner.board.ActivateTileSelection();
        owner.turnStatusUI.ActivateTurn(owner.enemyUnits[0].unitName);
        StartCoroutine(ApplyEvent());
    }


    IEnumerator ApplyEvent()
    {
        owner.turnStatusUI.IndicateTurnStatus(owner.turnStatusUI.monsterTurn);
        yield return new WaitForSeconds(1f);
        owner.turnStatusUI.StopTurnStatus();
        yield return new WaitForSeconds(1f);
        
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
