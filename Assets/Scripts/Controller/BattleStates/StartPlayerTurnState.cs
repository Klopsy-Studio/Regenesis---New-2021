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

        if(owner.currentUnit.debuffModifiers != null)
        {
            List<Modifier> trash = new List<Modifier>();

            foreach (Modifier m in owner.currentUnit.debuffModifiers)
            {
                trash.Add(m);
            }


            foreach (Modifier m in trash)
            {
                if (m.modifierType == TypeOfModifier.TimelineSpeed)
                {
                    owner.currentUnit.RemoveDebuff(m);
                }
            }
        }
        
        StartCoroutine(SetStats());

    }

    IEnumerator SetStats()
    {
        owner.currentUnit.TimelineVelocity = TimelineVelocity.VerySlow;
        owner.currentUnit.ActionsPerTurn = 5;
        owner.pauseTimelineButton.onUp.Invoke();

        owner.pauseTimelineButton.canBeSelected = false;
        owner.resumeTimelineButton.canBeSelected = false;

        yield return null;
        owner.ChangeState<SelectActionState>();
    }
}
