using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitUnitState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        
        StartCoroutine(PassTurnSequence());
    }

    IEnumerator PassTurnSequence()
    {
        owner.currentUnit.turnEnded = true;
        owner.actionSelectionUI.ResetSelector();
        owner.actionSelectionUI.gameObject.SetActive(false);
        yield return null;
        owner.ChangeState<FinishPlayerUnitTurnState>();
    }
}
