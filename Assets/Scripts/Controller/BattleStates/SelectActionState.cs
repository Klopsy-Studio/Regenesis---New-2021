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
        owner.currentUnit.GetComponent<Movement>().ResetRange();
      
        if (!owner.currentUnit.CanMove())
        {
            ActionSelectionUI.DisableSelectOption(typeOfAction.Move);
        }
        else
        {
            ActionSelectionUI.EnableSelectOption(typeOfAction.Move);     
        }

     
        if (!owner.currentUnit.CanDoAbility())
        {
            ActionSelectionUI.DisableSelectOption(typeOfAction.Ability);
        }
        else
        {
            ActionSelectionUI.EnableSelectOption(typeOfAction.Ability);
        }

        if(owner.currentUnit.actionsPerTurn >= 2)
        {
            ActionSelectionUI.EnableSelectOption(typeOfAction.Item);
        }
        else
        {
            ActionSelectionUI.DisableSelectOption(typeOfAction.Item);
        }
        //COMPROBAR SI SE UTILIZAR LOS ITEMS, SI NO SE PUEDE, QUE ESTE FADED
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
        owner.currentUnit = null;
        ActionSelectionUI.ResetSelector();
        ActionSelectionUI.gameObject.SetActive(false);
        currentAction = typeOfAction.Move;
        owner.ChangeState<SelectUnitState>();
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
        switch (e.info)
        {
            case 0:
                if (owner.currentUnit.CanMove())
                {
                    Debug.Log("CASE 0");
                    ActionSelectionUI.gameObject.SetActive(false);
                    owner.ChangeState<MoveTargetState>();
                }

                //owner.currentUnit.GetComponent<Movement>().PushUnit(Directions.South, 3, board);
                break;

            case 1:
                Debug.Log("CASE 1");

                if (owner.currentUnit.CanDoAbility())
                {
                    owner.ChangeState<SelectAbilityState>();
                }
                break;

            case 2:
                Debug.Log("CASE 2");
                //right now it will change to SelectItemState. That state will select the potion item automatically. 
                //we should change that in the future
                if(owner.currentUnit.actionsPerTurn >= 2)
                {
                    owner.ChangeState<SelectItemState>();
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
                break;

            case 5:
                owner.ChangeState<CheckUnitStatusState>();
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

                //owner.currentUnit.GetComponent<Movement>().PushUnit(Directions.South, 3, board);
                break;

            case typeOfAction.Ability:
                //if (owner.currentUnit.CanDoAbility())
                //{
                //    owner.ChangeState<SelectAbilityState>();
                //}
                owner.ChangeState<SelectAbilityState>();
                //OpenAbilityMenu
                break;

            case typeOfAction.Item:
                //right now it will change to SelectItemState. That state will select the potion item automatically. 
                //we should change that in the future
                owner.ChangeState<SelectItemState>();
                //OpenItemMenu
                break;

            case typeOfAction.Wait:
                //Skip turn
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
