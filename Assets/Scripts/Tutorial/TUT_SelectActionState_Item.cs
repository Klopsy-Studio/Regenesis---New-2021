using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_SelectActionState_Item : BattleState
{
    public typeOfAction currentAction = typeOfAction.Move;
    int test = 0;
    public override void Enter()
    {
        base.Enter();
        owner.board.ActivateTileSelectionToggle();

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

        owner.currentUnit.actionsPerTurn = 5;
        if (owner.currentUnit.hammerFuryAmount >= owner.currentUnit.hammerFuryMax)
        {
            owner.currentUnit.EnableHammerTrait();
        }

        ActionSelectionUI.DisableSelectOption(typeOfAction.Move);

        ActionSelectionUI.DisableSelectOption(typeOfAction.Ability);

        ActionSelectionUI.EnableSelectOption(typeOfAction.Item);

        ActionSelectionUI.DisableSelectOption(typeOfAction.Wait);
    }

    public override void Exit()
    {
        base.Exit();
        owner.currentUnit.unitSprite.gameObject.GetComponent<Renderer>().material.SetFloat("_OutlineThickness", 0);
        owner.moveActionSelector = false;
        owner.actionSelectionUI.DisableActionSelection();
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

    int stateIndex = 0;

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
                //    owner.ChangeState<MoveTargetState>();
                //}

                //owner.currentUnit.GetComponent<Movement>().PushUnit(Directions.South, 3, board);
                break;

            case 1:
                Debug.Log("CASE 1");

                //if (owner.currentUnit.CanDoAbility())
                //{
                //    owner.ChangeState<SelectAbilityState>();
                //}
                break;

            case 2:
                Debug.Log("CASE 2");
                //right now it will change to SelectItemState. That state will select the potion item automatically. 
                //we should change that in the future
                if (owner.currentUnit.actionsPerTurn >= 2)
                {
                    test++;
                    if(test == 1)
                    {
                        owner.ChangeState<TUT_SelectItemState>();
                    }
                    else
                    {
                        owner.ChangeState<TUT_SelectItemState_SelectPotion>();
                    }
                   
                }
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
                owner.ChangeState<CheckUnitStatusState>();
                break;

        }
    }
}
