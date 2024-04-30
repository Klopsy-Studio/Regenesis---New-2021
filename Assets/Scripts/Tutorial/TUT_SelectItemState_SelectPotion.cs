using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TUT_SelectItemState_SelectPotion : BattleState
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
            owner.itemSelectionUI.options[i].GetComponent<SelectorMovement>().controller = owner;
            owner.itemSelectionUI.options[i].GetComponent<TextMeshProUGUI>().text = "No Item";
            owner.itemSelectionUI.itemAmountText[i].GetComponent<TextMeshProUGUI>().text = "X";


            SelectorMovement e = owner.itemSelectionUI.options[i].GetComponent<SelectorMovement>();
            e.DisableOption();
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            var item = itemList[i];
            owner.itemSelectionUI.parent[i].gameObject.SetActive(true);
            owner.itemSelectionUI.options[i].GetComponent<TextMeshProUGUI>().text = itemList[i].consumable.itemName;
            owner.itemSelectionUI.itemAmountText[i].GetComponent<TextMeshProUGUI>().text = "x" + itemList[i].amount.ToString();

            SelectorMovement e = owner.itemSelectionUI.options[i].GetComponent<SelectorMovement>();
            e.assignedConsumable = itemList[i].consumable;
            e.UpdateTooltip();
            e.EnableOption();

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
