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

        owner.itemSelectionUI.gameObject.SetActive(true);
        ItemSelectionUI.ChangeAllItemsToDefault();
        //Abilities[] a = owner.currentUnit.weapon.Abilities;
        List<ConsumableSlot> itemList = owner.backpackInventory.consumableContainer;


        for (int i = 0; i < owner.itemSelectionUI.options.Length; i++)
        {
            owner.itemSelectionUI.options[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            var item = itemList[i];
            owner.itemSelectionUI.options[i].gameObject.SetActive(true);
            owner.itemSelectionUI.options[i].GetComponent<Text>().text = itemList[i].consumable.ItemName;
            owner.itemSelectionUI.itemAmountText[i].GetComponent<Text>().text = itemList[i].amount.ToString();
            owner.itemSelectionUI.itemImage[i].GetComponent<Image>().sprite = itemList[i].consumable.iconSprite;

            //Only for testing purposes
            if(item.amount == item.consumable.maxBackPackAmount)
            {
                owner.itemSelectionUI.itemAmountText[i].GetComponent<Text>().color = Color.green;
            }
            else if(item.amount < item.consumable.maxBackPackAmount)
            {
                owner.itemSelectionUI.itemAmountText[i].GetComponent<Text>().color = Color.black;
            }
            else if(item.amount > item.consumable.maxBackPackAmount)
            {
                owner.itemSelectionUI.itemAmountText[i].GetComponent<Text>().color = Color.red;
            }

            owner.itemSelectionUI.options[i].GetComponent<Text>().text = item.consumable.ItemName;
            owner.itemSelectionUI.itemAmountText[i].GetComponent<Text>().text = item.amount.ToString();
            owner.itemSelectionUI.itemImage[i].sprite = item.consumable.iconSprite;
            
            owner.itemSelectionUI.itemImage[i].SetNativeSize();
            SelectorMovement e = owner.itemSelectionUI.options[i].GetComponent<SelectorMovement>();
            e.canBeSelected = true;
            e.abilityDescription.AssignData(itemList[i].consumable);
            e.abilityDescription.gameObject.SetActive(false);
        }


        owner.itemSelectionUI.ResetSelector();
        //Meter ActivarUI
    }

    protected override void OnEscape(object sender, InfoEventArgs<KeyCode> e)
    {

        owner.ChangeState<SelectActionState>();
    }
    protected override void OnMouseCancelEvent(object sender, InfoEventArgs<KeyCode> e)
    {
        owner.ChangeState<SelectActionState>();
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


    public override void Exit()
    {
        base.Exit();
        owner.moveItemSelector = false;
        currentItemIndex = 0;
        owner.itemSelectionUI.ResetSelector();
        owner.itemSelectionUI.gameObject.SetActive(false);
    }

   

    //IEnumerator Init()
    //{
    //    Debug.Log("se ha utilizado Potion");
    //    yield return null;
    //    owner.inventory.UseConsumable(0, owner.currentUnit);
    //    owner.ChangeState<SelectActionState>();

    //}


}


