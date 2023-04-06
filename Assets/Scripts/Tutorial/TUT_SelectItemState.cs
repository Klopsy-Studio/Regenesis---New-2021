using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TUT_SelectItemState : BattleState
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
            owner.itemSelectionUI.parent[i].gameObject.SetActive(true);
            owner.itemSelectionUI.options[i].GetComponent<Text>().text = itemList[i].consumable.itemName;
            owner.itemSelectionUI.itemAmountText[i].GetComponent<Text>().text = "x" + itemList[i].amount.ToString();
            owner.itemSelectionUI.itemImage[i].GetComponent<Image>().sprite = itemList[i].consumable.iconSprite;

            //Only for testing purposes
            //if(item.amount == item.consumable.maxBackPackAmount)
            //{
            //    owner.itemSelectionUI.itemAmountText[i].GetComponent<Text>().color = Color.green;
            //}
            //else if(item.amount < item.consumable.maxBackPackAmount)
            //{
            //    owner.itemSelectionUI.itemAmountText[i].GetComponent<Text>().color = Color.black;
            //}
            //else if(item.amount > item.consumable.maxBackPackAmount)
            //{
            //    owner.itemSelectionUI.itemAmountText[i].GetComponent<Text>().color = Color.red;
            //}

            owner.itemSelectionUI.options[i].GetComponent<Text>().text = item.consumable.itemName;
            owner.itemSelectionUI.itemAmountText[i].GetComponent<Text>().text = item.amount.ToString();
            owner.itemSelectionUI.itemImage[i].sprite = item.consumable.iconSprite;

            owner.itemSelectionUI.itemImage[i].SetNativeSize();
            if (i != 0)
            {
                SelectorMovement e = owner.itemSelectionUI.options[i].GetComponent<SelectorMovement>();
                e.assignedConsumable = itemList[i].consumable;
                e.canBeSelected = false;
                
            }
            else
            {
                SelectorMovement e = owner.itemSelectionUI.options[i].GetComponent<SelectorMovement>();
                e.assignedConsumable = itemList[i].consumable;
                e.canBeSelected = true;
            }

        }
        Debug.Log("Ha entrado a TUT SELECTITEMSTATE");

        owner.itemSelectionUI.ResetSelector();
        //Meter ActivarUI
    }

 

    protected override void OnSelectAction(object sender, InfoEventArgs<int> e)
    {
        //owner.inventory.UseConsumable(e.info, owner.currentUnit);
        //owner.ChangeState<FinishPlayerUnitTurnState>();
        if (ItemSelectionUI.options[e.info].GetComponent<SelectorMovement>().canBeSelected)
        {
            owner.itemChosen = e.info;
            owner.ChangeState<TUT_UseItemState>(); 
        }
        //owner.itemChosen = e.info;
        //owner.ChangeState<TUT_UseItemState>();
    }


    public override void Exit()
    {
        base.Exit();
        owner.moveItemSelector = false;
        currentItemIndex = 0;
        owner.itemSelectionUI.ResetSelector();
    }

}
