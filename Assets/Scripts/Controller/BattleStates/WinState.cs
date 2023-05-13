using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.canPause = false;
        owner.unitStatusUI.gameObject.SetActive(false);
        owner.turnStatusUI.gameObject.SetActive(false);
        owner.timelineUI.gameObject.SetActive(false);
        owner.actionSelectionUI.gameObject.SetActive(false);
        owner.miniStatus.gameObject.SetActive(false);
        owner.turnArrow.gameObject.SetActive(false);
        owner.canToggleTimeline = false;
        tileSelectionIndicator.gameObject.SetActive(false);
        owner.isTimeLineActive = false;
        owner.battleEnded = true;

        StartCoroutine(Win());
    }


    IEnumerator Win()
    {
        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }
        owner.actionSelectionUI.gameObject.SetActive(false);
        owner.abilitySelectionUI.gameObject.SetActive(false);
        owner.itemSelectionUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.3f);
        owner.turnStatusUI.IndicateTurnStatus(owner.turnStatusUI.winTurn);
        owner.actionSelectionUI.gameObject.SetActive(false);
        owner.abilitySelectionUI.gameObject.SetActive(false);
        owner.itemSelectionUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.3f);
        owner.turnStatusUI.DeactivateTurn();
        yield return new WaitForSeconds(0.5f);
        owner.levelData.hasBeenCompleted = true;

        //Switch later to show Loot load camp scene 
        //SceneManager.LoadScene("Battle");
        owner.ChangeState<LootUIState>();
    }
}
