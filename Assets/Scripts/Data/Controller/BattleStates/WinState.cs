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
        owner.partyIconParent.gameObject.SetActive(false);
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
        owner.battleContextControls.gameObject.SetActive(false);
        StartCoroutine(Win());
    }


    IEnumerator Win()
    {
        owner.levelData.CompleteHunt();

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }
        owner.actionSelectionUI.gameObject.SetActive(false);
        owner.abilitySelectionUI.gameObject.SetActive(false);
        owner.itemSelectionUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        AudioManager.instance.Play("MainTheme");

        owner.actionSelectionUI.gameObject.SetActive(false);
        owner.abilitySelectionUI.gameObject.SetActive(false);
        owner.itemSelectionUI.gameObject.SetActive(false);

        owner.ChangeState<LootUIState>();
    }
}
