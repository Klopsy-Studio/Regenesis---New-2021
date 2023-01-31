using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestUnitState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(RestSequence());
    }

    IEnumerator RestSequence()
    {
       
        owner.currentUnit.turnEnded = true;

        owner.actionSelectionUI.ResetSelector();
        owner.actionSelectionUI.gameObject.SetActive(false);

        yield return null;
        owner.ChangeState<FinishPlayerUnitTurnState>();
    }
}
