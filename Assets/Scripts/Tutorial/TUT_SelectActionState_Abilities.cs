using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_SelectActionState_Abilities : BattleState
{
    public typeOfAction currentAction = typeOfAction.Move;
    public override void Enter()
    {
        base.Enter();
        Debug.Log("ESTAS EN TUT_SELECT ACTION MOVE");
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

      

        ActionSelectionUI.DisableSelectOption(typeOfAction.Move);
        ActionSelectionUI.DisableSelectOption(typeOfAction.Item);
        ActionSelectionUI.DisableSelectOption(typeOfAction.Wait);
        ActionSelectionUI.DisableSelectOption(typeOfAction.Status);
        //if (!owner.currentUnit.CanDoAbility())
        //{
        //    ActionSelectionUI.DisableSelectOption(typeOfAction.Ability);
        //}
        //else
        //{
        //    ActionSelectionUI.EnableSelectOption(typeOfAction.Ability);
        //}

        //if (owner.currentUnit.actionsPerTurn >= 2)
        //{
        //    ActionSelectionUI.EnableSelectOption(typeOfAction.Item);
        //}
        //else
        //{
        //    ActionSelectionUI.DisableSelectOption(typeOfAction.Item);
        //}


        if (owner.currentUnit.hammerFuryAmount >= owner.currentUnit.hammerFuryMax)
        {
            owner.currentUnit.EnableHammerTrait();
        }
    }

    public override void Exit()
    {
        base.Exit();
        owner.currentUnit.unitSprite.gameObject.GetComponent<Renderer>().material.SetFloat("_OutlineThickness", 0);
        owner.moveActionSelector = false;
        owner.actionSelectionUI.DisableActionSelection();
    }

    protected override void OnEscape(object sender, InfoEventArgs<KeyCode> e)
    {
        //owner.currentUnit = null;
        //ActionSelectionUI.ResetSelector();
        //ActionSelectionUI.gameObject.SetActive(false);
        //currentAction = typeOfAction.Move;
        //owner.ChangeState<SelectUnitState>();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if (e.info.y >= 1)
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

        if (e.info.y <= -1)
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
                //if (owner.currentUnit.CanMove())
                //{
                //    Debug.Log("CASE 0");
                //    ActionSelectionUI.gameObject.SetActive(false);
                //    owner.ChangeState<TUT_MoveTargeStateOne>();
                //}

                //owner.currentUnit.GetComponent<Movement>().PushUnit(Directions.South, 3, board);
                break;

            case 1:
                Debug.Log("CASE 1");

                if (owner.currentUnit.CanDoAbility())
                {
                    owner.ChangeState<TUT_SelectAbilityStateOne>();
                }
                break;

            case 2:
                Debug.Log("CASE 2");
                //right now it will change to SelectItemState. That state will select the potion item automatically. 
                //we should change that in the future
                //if (owner.currentUnit.actionsPerTurn >= 2)
                //{
                //    owner.ChangeState<SelectItemState>();
                //}
                //OpenItemMenu
                break;

            case 3:
                //Recover stamina and end turn
                //if (!owner.currentUnit.actionDone)
                //{
                //    currentAction = typeOfAction.Move;
                //    owner.ChangeState<RestUnitState>();
                //}
                break;
            case 4:
                //Skip turn

                //owner.ChangeState<WaitUnitState>();
                break;

            case 5:
                //owner.ChangeState<CheckUnitStatusState>();
                break;

        }
    }
    protected override void OnFire(object sender, InfoEventArgs<KeyCode> e)
    {

        switch (currentAction)
        {
            case typeOfAction.Move:
                if (owner.currentUnit.CanMove())
                {
                    ActionSelectionUI.gameObject.SetActive(false);

                    owner.ChangeState<MoveTargetState>();
                }
                break;

            case typeOfAction.Ability:
                owner.ChangeState<SelectAbilityState>();
                break;

            case typeOfAction.Item:
                owner.ChangeState<SelectItemState>();
                break;

            case typeOfAction.Wait:
                currentAction = typeOfAction.Move;
                owner.ChangeState<WaitUnitState>();
                break;
            case typeOfAction.Status:
                currentAction = typeOfAction.Status;
                owner.ChangeState<CheckUnitStatusState>();
                break;

        }
    }
}
