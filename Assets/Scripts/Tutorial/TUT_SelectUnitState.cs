using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_SelectUnitState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.StartAction();
        //StartCoroutine(CanAct());
        Debug.Log("entra a tut_selectUnitState");
        StartCoroutine(SelectUnitCoroutine());
    }

    IEnumerator SelectUnitCoroutine()
    {
        StartCoroutine(owner.IndicateTurn(owner.turnStatusUI.playerTurn));

        while (owner.indicatingTurn)
        {
            yield return null;
        }

        SelectTile(owner.currentUnit.currentPoint);
        yield return null;
        owner.ChangeState<TUT_StartPlayerTurnState>();
    }
}
