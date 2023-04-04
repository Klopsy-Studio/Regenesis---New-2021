using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_RestUnitStateOne : BattleState
{

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Ha entrado a tut rest unitstate one");
        StartCoroutine(RestSequence());
    }

    IEnumerator RestSequence()
    {

        owner.currentUnit.turnEnded = true;

        owner.actionSelectionUI.ResetSelector();
        owner.actionSelectionUI.gameObject.SetActive(false);

        yield return null;
        owner.ChangeState<TUT_FinishPlayerUnitTurnStateOne>();
    }
}
