using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUnitStatusState : BattleState
{

    public override void Enter()
    {
        base.Enter();
        owner.actionSelectionUI.gameObject.SetActive(false);
        owner.expandedUnitStatus.AssignValues(owner.currentUnit);
        owner.expandedUnitStatus.gameObject.SetActive(true);
    }

    protected override void OnMouseCancelEvent(object sender, InfoEventArgs<KeyCode> e)
    {
        base.OnMouseCancelEvent(sender, e);
        owner.ChangeState<SelectActionState>();
    }

    public override void Exit()
    {
        base.Exit();
        owner.expandedUnitStatus.gameObject.SetActive(false);
    }
}
