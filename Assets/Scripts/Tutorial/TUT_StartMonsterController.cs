using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_StartMonsterController : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.isTimeLineActive = false;
        owner.StartAction();
        owner.board.ActivateTileSelection();
        owner.turnStatusUI.ActivateTurn(owner.currentEnemyUnit.unitName);
        //tileSelectionIndicator.gameObject.SetActive(false);
        owner.miniStatus.SetStatus(owner.currentEnemyUnit);
        List<Modifier> trash = new List<Modifier>();

        foreach (Modifier m in owner.currentEnemyUnit.debuffModifiers)
        {
            trash.Add(m);
        }

        foreach (Modifier m in trash)
        {
            if (m.modifierType == TypeOfModifier.TimelineSpeed)
            {
                owner.currentEnemyUnit.RemoveDebuff(m);
            }
        }

        //StartCoroutine(StartEnemyTurnCoroutine());
        StartCoroutine(StartEnemyTurnCoroutine());
    }

    IEnumerator StartEnemyTurnCoroutine()
    {
        yield return null;

        owner.monsterController.StartMonster();
    }
}
