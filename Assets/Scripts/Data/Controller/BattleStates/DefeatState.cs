using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.canPause = false;

        owner.unitStatusUI.gameObject.SetActive(false);
        owner.turnStatusUI.gameObject.SetActive(false);
        owner.timelineUI.gameObject.SetActive(false);
        owner.partyIconParent.gameObject.SetActive(false);
        tileSelectionIndicator.gameObject.SetActive(false);
        owner.battleEnded = true;
        owner.isTimeLineActive = false;
        StartCoroutine(LoseState());
    }


    IEnumerator LoseState()
    {
        owner.actionSelectionUI.gameObject.SetActive(false);
        owner.abilitySelectionUI.gameObject.SetActive(false);
        owner.itemSelectionUI.gameObject.SetActive(false);
        owner.miniStatus.gameObject.SetActive(false);
        owner.turnStatusUI.IndicateTurnStatus(owner.turnStatusUI.loseTurn);
        yield return new WaitForSeconds(1);
        owner.turnStatusUI.DeactivateTurn();

        AudioManager.instance.Play("LoseTheme");

        yield return new WaitForSeconds(1);


        //Deactivated for now, if we want to show loot screen later on we should define what happens when you lose
        //owner.ChangeState<LootUIState>();
 
        owner.ChangeState<DefeatUIState>();
    }

}
