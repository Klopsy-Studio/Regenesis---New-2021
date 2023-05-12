using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_FinishPlayerUnitTurnStateOne : BattleState
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("ha entrado a tut_finishplayerunitturn state");
        StartCoroutine(FinishTurnCoroutine());
    }

    public override void Exit()
    {
        base.Exit();
        owner.isTimeLineActive = true;
    }

    IEnumerator FinishTurnCoroutine()
    {
        List<Modifier> trashModifier = new List<Modifier>();

        if (owner.currentUnit.buffModifiers.Count > 0)
        {
            foreach (Modifier m in owner.currentUnit.buffModifiers)
            {
                if (m.modifierType == TypeOfModifier.PiercingSharpness)
                {
                    owner.currentUnit.criticalDamage = 1.5f;
                    owner.currentUnit.criticalPercentage = owner.currentUnit.weapon.criticalPercentage;
                    trashModifier.Add(m);
                }
            }
        }

        if (trashModifier.Count > 0)
        {
            foreach (Modifier m in trashModifier)
            {
                owner.currentUnit.RemoveBuff(m);
            }
        }

        owner.currentUnit.ResetWeaponTraits();
        owner.currentUnit.SetVelocityWhenTurnIsFinished();
        owner.turnArrow.DeactivateTarget();
        //Debug.Log("CURRENT VELOCITY ES " + owner.currentUnit.TimelineVelocity + " CURRENT UNIT ACTIONS " + owner.currentUnit.ActionsPerTurn);
        owner.currentUnit.didNotMove = true;
        owner.currentUnit.timelineFill = 0;
        //owner.currentUnit.status.ChangeToSmall();
        owner.miniStatus.DeactivateStatus();
        owner.currentUnit.playerUI.HideActionPoints();
        owner.currentUnit.iconTimeline.SetTimelineIconTextVelocity();
        owner.board.DeactivateTileSelection();

        owner.pauseTimelineButton.canBeSelected = true;
        owner.resumeTimelineButton.canBeSelected = true;
        owner.resumeTimelineButton.onUp.Invoke();

        owner.timelineUI.ShowTimelineIcon(owner.currentUnit);
        //AudioManager.instance.Play("TurnEnd");
        yield return null;
        owner.ChangeState<TUT_TimelineStateFirstHunter>();
    }
}
