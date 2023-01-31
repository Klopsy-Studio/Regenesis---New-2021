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
        owner.board.ActivateTileSelection();
        owner.turnStatusUI.ActivateTurn("Ursamyr");
        //tileSelectionIndicator.gameObject.SetActive(false);
       
     
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
        owner.isTimeLineActive = false;
        owner.turnStatusUI.IndicateTurnStatus(owner.turnStatusUI.monsterTurn);
        yield return new WaitForSeconds(1f);
        owner.turnStatusUI.StopTurnStatus();
        yield return new WaitForSeconds(1f);

        owner.monsterController.StartMonster(); 
    }


}
