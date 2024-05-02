using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typeOfAction
{
    Move,
    Ability,
    Item,
    Wait,
    Status,
};
public class SelectActionState : BattleState
{
    public typeOfAction currentAction = typeOfAction.Move;
    public override void Enter()
    {
        base.Enter();
        if (!owner.battleEnded)
        {
            owner.turnArrow.SetTarget(owner.currentUnit.currentPoint, 3.5f);
            owner.miniStatus.SetStatus(owner.currentUnit);

            owner.SelectTile(owner.currentUnit.currentPoint);
            owner.tileSelectionToggle.MakeTileSelectionSmall();
            owner.DeactivateTileSelector();

            owner.currentUnit.playerUI.unitUI.gameObject.SetActive(true);
            owner.currentUnit.playerUI.ShowActionPoints();

            owner.currentUnit.unitSprite.gameObject.GetComponent<Renderer>().material.SetFloat("_OutlineThickness", 1);
            owner.isTimeLineActive = false;
            owner.moveActionSelector = true;
            owner.actionSelectionUI.gameObject.SetActive(true);
            owner.actionSelectionUI.ChangeAllActionsToDefault();
            owner.actionSelectionUI.EnableActionSelection();
            owner.actionSelectionUI.OriginalColor();
            owner.actionSelectionUI.title.SetActive(true);
            owner.abilitySelectionUI.gameObject.SetActive(false);
            owner.itemSelectionUI.gameObject.SetActive(false);

            owner.currentUnit.GetComponent<Movement>().ResetRange();

            if (!owner.currentUnit.CanMove())
            {
                ActionSelectionUI.DisableSelectOption(typeOfAction.Move);
            }
            else
            {
                ActionSelectionUI.EnableSelectOption(typeOfAction.Move);
                ActionSelectionUI.GetMovementOption().GetPreviewRange();
            }

            if (!owner.currentUnit.CanDoAbility())
            {
                ActionSelectionUI.DisableSelectOption(typeOfAction.Ability);
            }
            else
            {
                ActionSelectionUI.EnableSelectOption(typeOfAction.Ability);
            }

            if (CanUseItems())
            {
                ActionSelectionUI.EnableSelectOption(typeOfAction.Item);
            }
            else
            {
                ActionSelectionUI.DisableSelectOption(typeOfAction.Item);
            }


            if (owner.currentUnit.hammerFuryAmount >= owner.currentUnit.hammerFuryMax)
            {
                owner.currentUnit.EnableHammerTrait();
            }
        }

        owner.ChangeCurrentControls("Actions");

    }

    public override void Exit()
    {
        base.Exit();
        owner.currentUnit.unitSprite.gameObject.GetComponent<Renderer>().material.SetFloat("_OutlineThickness", 0);
        owner.moveActionSelector = false;
        owner.actionSelectionUI.DisableActionSelection();
    }


    public bool CanUseItems()
    {
        if(owner.backpackInventory.consumableContainer.Count >0 && owner.currentUnit.actionsPerTurn >= owner.itemCost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    

   

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if(e.info.y >= 1)
        {
            ActionSelectionUI.MoveBackwards();

            switch (currentAction)
            {
                case typeOfAction.Move:
                    currentAction = typeOfAction.Status;
                    break;
                case typeOfAction.Ability:
                    currentAction = typeOfAction.Move;
                    break;
                case typeOfAction.Item:
                    currentAction = typeOfAction.Ability;
                    break;
                case typeOfAction.Wait:
                    currentAction = typeOfAction.Item;
                    break;
                case typeOfAction.Status:
                    currentAction = typeOfAction.Wait;
                    break;
                default:
                    break;
            }
        }

        if(e.info.y <= -1)
        {
            ActionSelectionUI.MoveForward();
            switch (currentAction)
            {
                case typeOfAction.Move:
                    currentAction = typeOfAction.Ability;
                    break;
                case typeOfAction.Ability:
                    currentAction = typeOfAction.Item;
                    break;
                case typeOfAction.Item:
                    currentAction = typeOfAction.Wait;
                    break;
                case typeOfAction.Wait:
                    currentAction = typeOfAction.Status;
                    break;
                case typeOfAction.Status:
                    currentAction = typeOfAction.Move;
                    break;
                default:
                    break;
            }
        }
    }


    protected override void OnSelectAction(object sender, InfoEventArgs<int> e)
       {
        owner.actionSelectionUI.actionDescription.SetActive(false);
        switch (e.info)
        {
            case 0:
                if (owner.currentUnit.CanMove())
                {
                    Debug.Log("CASE 0");
                    ActionSelectionUI.gameObject.SetActive(false);
                    owner.ChangeState<MoveTargetState>();
                    AudioManager.instance.Play("Boton" + owner.enterMenu);
                }

                //owner.currentUnit.GetComponent<Movement>().PushUnit(Directions.South, 3, board);
                break;

            case 1:
                Debug.Log("CASE 1");

                if (owner.currentUnit.CanDoAbility())
                {
                    owner.ChangeState<SelectAbilityState>();
                    AudioManager.instance.Play("Boton" + owner.enterMenu);

                }
                break;

            case 2:
                Debug.Log("CASE 2");
                //right now it will change to SelectItemState. That state will select the potion item automatically. 
                //we should change that in the future
                if(CanUseItems())
                {
                    owner.ChangeState<SelectItemState>();
                    AudioManager.instance.Play("Boton" + owner.enterMenu);

                }
                //OpenItemMenu
                break;

            case 3:
                //Recover stamina and end turn
                if (!owner.currentUnit.actionDone)
                {
                    currentAction = typeOfAction.Move;
                    owner.ChangeState<RestUnitState>();
                }
                break;
            case 4:
                //Skip turn

                owner.ChangeState<WaitUnitState>();
                AudioManager.instance.Play("Boton" + owner.enterMenu);

                break;

            case 5:
                owner.ChangeState<CheckUnitStatusState>();
                break;

        }
    }


}
