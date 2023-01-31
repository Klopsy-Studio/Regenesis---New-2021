using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.unitStatusUI.gameObject.SetActive(false);
        owner.turnStatusUI.gameObject.SetActive(false);
        owner.timelineUI.gameObject.SetActive(false);
        tileSelectionIndicator.gameObject.SetActive(false);
        owner.isTimeLineActive = false;

        StartCoroutine(Win());
    }


    IEnumerator Win()
    {
        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        owner.turnStatusUI.IndicateTurnStatus(owner.turnStatusUI.winTurn);
        yield return new WaitForSeconds(1);
        owner.turnStatusUI.DeactivateTurn();
        yield return new WaitForSeconds(1);
        owner.levelData.hasBeenCompleted = true;

        //Switch later to show Loot load camp scene 
        //SceneManager.LoadScene("Battle");
        owner.ChangeState<LootUIState>();
    }
}
