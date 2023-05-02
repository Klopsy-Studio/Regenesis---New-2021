using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDroneTarget : BattleState
{
    PlayerUnit currentTarget;
    bool selecting = false;
    public override void Enter()
    {
        base.Enter();
        owner.abilitySelectionUI.gameObject.SetActive(false);
        owner.actionSelectionUI.gameObject.SetActive(false);
        owner.targets.gameObject.SetActive(true);
        owner.targets.stopSelection = false;
        owner.targets.CreateDroneTargets();
    }


    protected override void OnMouseConfirm(object sender, InfoEventArgs<KeyCode> e)
    {
        if(owner.targets.selectedTarget != null && !selecting)
        {
            owner.targets.gameObject.SetActive(false);
            currentTarget = owner.targets.selectedTarget.targetAssigned.GetComponent<PlayerUnit>();
            selecting = true;
            StartCoroutine(SetCurrentTargetSequence());
        }
    }

    protected override void OnMouseCancelEvent(object sender, InfoEventArgs<KeyCode> e)
    {
        if (!selecting)
        {
            owner.ChangeState<SelectAbilityState>();
        }
    }
    IEnumerator SetCurrentTargetSequence()
    {
        if (owner.currentUnit.droneUnit != null)
        {
            owner.SelectTile(owner.currentUnit.droneUnit.tile.pos);
            yield return new WaitForSeconds(0.2f);

            owner.currentUnit.droneUnit.DisableDrone();
            yield return new WaitForSeconds(0.5f);
        }

        owner.SelectTile(currentTarget.tile.pos);
        owner.currentUnit.droneUnit = currentTarget;
        yield return new WaitForSeconds(0.2f);

        currentTarget.EnableDrone();

        yield return new WaitForSeconds(0.5f);

        owner.SelectTile(owner.currentUnit.tile.pos);

        owner.ChangeState<SelectAbilityState>();

    }

    public void HighlightTarget()
    {
        currentTarget.unitSprite.color = new Color(currentTarget.unitSprite.color.r, currentTarget.unitSprite.color.g, currentTarget.unitSprite.color.b, 1f);
    }

    public void ReturnTarget()
    {
        currentTarget.unitSprite.color = new Color(currentTarget.unitSprite.color.r, currentTarget.unitSprite.color.g, currentTarget.unitSprite.color.b, 0.5f);
    }
    public override void Exit()
    {
        base.Exit();
        owner.targets.ClearTargets();
        selecting = false;
        owner.targets.gameObject.SetActive(false);
    }
}
