using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.unitStatusUI.gameObject.SetActive(false);
        owner.turnStatusUI.gameObject.SetActive(false);
        owner.timelineUI.gameObject.SetActive(false);
        tileSelectionIndicator.gameObject.SetActive(false);
        owner.battleEnded = true;
        owner.isTimeLineActive = false;
        StartCoroutine(LoseState());
    }


    IEnumerator LoseState()
    {
        owner.turnStatusUI.IndicateTurnStatus(owner.turnStatusUI.loseTurn);
        yield return new WaitForSeconds(1);
        owner.turnStatusUI.DeactivateTurn();
        yield return new WaitForSeconds(1);


        //Deactivated for now, if we want to show loot screen later on we should define what happens when you lose
        //owner.ChangeState<LootUIState>();

        owner.pauseButton.canBeSelected = false;
        owner.defeatScreen.SetActive(true);
    }

}
