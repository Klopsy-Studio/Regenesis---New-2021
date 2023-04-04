using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TUT_StartPlayerTurnState : BattleState
{
    int stateIndex = 0;
    public override void Enter()
    {
        base.Enter();


        //Disabling Mini Staus
        //owner.currentUnit.status.ChangeToBig();
        Debug.Log("?");
        owner.turnStatusUI.ActivateTurn(owner.currentUnit.unitName);
        owner.board.ActivateTileSelection();

        owner.miniStatus.SetStatus(owner.currentUnit);

        if (owner.currentUnit.debuffModifiers != null)
        {
            if (owner.currentUnit.debuffModifiers.Count > 0)
            {
                List<Modifier> trash = new List<Modifier>();

                foreach (Modifier m in owner.currentUnit.debuffModifiers)
                {
                    if (m.modifierType == TypeOfModifier.TimelineSpeed)
                    {
                        trash.Add(m);
                    }
                }


                foreach (Modifier m in trash)
                {
                    owner.currentUnit.RemoveDebuff(m);
                }
            }

        }


        if (owner.currentUnit.buffModifiers != null)
        {
            if (owner.currentUnit.buffModifiers.Count > 0)
            {
                List<Modifier> trash = new List<Modifier>();

                foreach (Modifier m in owner.currentUnit.buffModifiers)
                {
                    if (m.modifierType == TypeOfModifier.TimelineSpeed)
                    {
                        trash.Add(m);
                    }
                }


                foreach (Modifier m in trash)
                {
                    owner.currentUnit.RemoveBuff(m);
                }
            }

        }

        StartCoroutine(SetStats());

    }

    IEnumerator SetStats()
    {
        owner.currentUnit.TimelineVelocity = TimelineVelocity.VerySlow;
        owner.currentUnit.ActionsPerTurn = 5;
        yield return null;
        stateIndex++;
        if (stateIndex == 1)
        {
            owner.ChangeState<TUT_SelectActionState_Move>();
        }
        else if(stateIndex == 2)
        {
            Debug.Log("YEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            owner.ChangeState<TUT_SelectActionState_Abilities>();
        }
       
    }
}
