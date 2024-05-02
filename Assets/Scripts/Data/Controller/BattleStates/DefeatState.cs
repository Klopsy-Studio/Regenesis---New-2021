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
        owner.battleContextControls.gameObject.SetActive(false);

        StartCoroutine(LoseState());
    }


    IEnumerator LoseState()
    {
        owner.actionSelectionUI.gameObject.SetActive(false);
        owner.abilitySelectionUI.gameObject.SetActive(false);
        owner.itemSelectionUI.gameObject.SetActive(false);
        owner.miniStatus.gameObject.SetActive(false);

        AudioManager.instance.Play("LoseTheme");

        yield return new WaitForSeconds(0.5f);

        owner.ChangeState<DefeatUIState>();
    }

}
