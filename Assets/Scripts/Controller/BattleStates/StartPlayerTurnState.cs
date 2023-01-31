using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayerTurnState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.currentUnit.status.ChangeToBig();
        Debug.Log("?");
        owner.turnStatusUI.ActivateTurn(owner.currentUnit.unitName);
        owner.board.ActivateTileSelection();
        StartCoroutine(SetStats());

    }

    IEnumerator SetStats()
    {
        owner.currentUnit.TimelineVelocity = TimelineVelocity.VerySlow;
        owner.currentUnit.ActionsPerTurn = 5;

        yield return null;
        owner.ChangeState<SelectActionState>();
    }
}
