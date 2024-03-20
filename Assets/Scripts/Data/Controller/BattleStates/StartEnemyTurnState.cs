using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnemyTurnState : BattleState
{
    //Estado en que se elige al enemigo

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

        owner.DeactivateCurrentControls();
        //StartCoroutine(StartEnemyTurnCoroutine());
        StartCoroutine(StartEnemyTurnCoroutine());
    }

    //IEnumerator StartEnemyTurnCoroutine()
    //{
    //    yield return null;
    //    owner.currentEnemyController.StartEnemy();
    //}


    IEnumerator StartEnemyTurnCoroutine()
    {
        if(owner.currentEntity != CurrentEntityTurn.Monster)
        {
            StartCoroutine(owner.IndicateTurn(owner.turnStatusUI.monsterTurn));

            while (owner.indicatingTurn)
            {
                yield return null;
            }
        }

        owner.currentEntity = CurrentEntityTurn.Monster;
        owner.monsterController.StartMonster(); 
    }


}
