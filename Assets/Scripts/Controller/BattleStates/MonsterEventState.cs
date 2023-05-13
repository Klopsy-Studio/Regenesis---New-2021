using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEventState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.isTimeLineActive = false;
        owner.StartAction();
        owner.board.ActivateTileSelection();
        owner.turnStatusUI.ActivateTurn(owner.enemyUnits[0].unitName);
        StartCoroutine(ApplyEvent());
    }


    IEnumerator ApplyEvent()
    {
        StartCoroutine(owner.IndicateTurn(owner.turnStatusUI.monsterTurn));
        while (owner.indicatingTurn)
        {
            yield return null;
        }

        owner.currentMonsterEvent.Apply();

        while (owner.currentMonsterEvent.acting)
        {
            yield return null;
        }

        owner.currentMonsterEvent.elementEnabled = false;
        owner.timelineElements.Remove(owner.currentMonsterEvent);
        owner.currentMonsterEvent = null;

        owner.timelineUI.HideIconActing();

        yield return new WaitForSeconds(0.5f);


        owner.FinishAction();

        owner.ChangeState<TimeLineState>();
    }
}
