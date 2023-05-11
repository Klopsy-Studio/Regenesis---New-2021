using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItemState : BattleState
{
    int currentItemIndex;
    public override void Enter()
    {
        base.Enter();
        owner.isTimeLineActive = false;
        owner.moveItemSelector = true;
        if (ActionSelectionUI.gameObject.activeSelf == false)
        {
            ActionSelectionUI.gameObject.SetActive(true);
        }

        owner.actionSelectionUI.SecondWindow();
        owner.actionSelectionUI.title.SetActive(false);
        owner.itemSelectionUI.title.SetActive(true);
        owner.itemSelectionUI.gameObject.SetActive(true);
        owner.itemSelectionUI.OriginalColor();
        ItemSelectionUI.ChangeAllItemsToDefault();
        //Abilities[] a = owner.currentUnit.weapon.Abilities;
        List<ConsumableSlot> itemList = owner.backpackInventory.consumableContainer;


        for (int i = 0; i < owner.itemSelectionUI.options.Length; i++)
        {
            owner.itemSelectionUI.parent[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            var item = itemList[i];


            owner.itemSelectionUI.options[i].GetComponent<Text>().text = item.consumable.itemName;
            owner.itemSelectionUI.itemAmountText[i].GetComponent<Text>().text = item.amount.ToString();

                       
            SelectorMovement e = owner.itemSelectionUI.options[i].GetComponent<SelectorMovement>();
            e.assignedConsumable = itemList[i].consumable;
            e.canBeSelected = true;

        }


        owner.itemSelectionUI.ResetSelector();
        //Meter ActivarUI
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if (e.info.y >= 1)
        {
            owner.itemSelectionUI.MoveBackwards();

            if (currentItemIndex > 0)
            {
                currentItemIndex--;
            }
            else
            {
                //???
                //currentActionIndex = owner.currentUnit.weapon.Abilities.Length - 1;
                currentItemIndex = owner.backpackInventory.consumableContainer.Count - 1;
            }

        }

        if (e.info.y <= -1)
        {
            owner.itemSelectionUI.MoveForward();

            if (currentItemIndex < owner.backpackInventory.consumableContainer.Count - 1)
            {
                currentItemIndex++;
            }
            else
            {
                currentItemIndex = 0;
            }
        }
    }

    protected override void OnFire(object sender, InfoEventArgs<KeyCode> e)
    {
        //if (owner.currentUnit.weapon.Abilities[currentActionIndex].CanBeUsed(owner.currentUnit.stamina))
        //{
        //    owner.attackChosen = currentActionIndex;
        //    ActionSelectionUI.gameObject.SetActive(false);
        //    owner.ChangeState<UseAbilityState>();
        //}


        //owner.inventory.UseConsumable(currentActionIndex, owner.currentUnit);
        //owner.ChangeState<FinishPlayerUnitTurnState>();

        owner.itemChosen = currentItemIndex;
        owner.ChangeState<UseItemState>();
    }

    protected override void OnSelectAction(object sender, InfoEventArgs<int> e)
    {
        //owner.inventory.UseConsumable(e.info, owner.currentUnit);
        //owner.ChangeState<FinishPlayerUnitTurnState>();

        owner.itemChosen = e.info;
        owner.ChangeState<UseItemState>();
    }

    protected override void OnSelectCancelEvent(object sender, InfoEventArgs<int> e)
    {
        owner.ChangeState<SelectActionState>();
    }
    public override void Exit()
    {
        base.Exit();
        owner.moveItemSelector = false;
        currentItemIndex = 0;
        owner.itemSelectionUI.ResetSelector();
    }

   

    //IEnumerator Init()
    //{
    //    Debug.Log("se ha utilizado Potion");
    //    yield return null;
    //    owner.inventory.UseConsumable(0, owner.currentUnit);
    //    owner.ChangeState<SelectActionState>();

    //}


}


