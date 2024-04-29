using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectItemState : BattleState
{
    int currentItemIndex;
    public override void Enter()
    {
        owner.ChangeCurrentControls("Items");

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
            owner.itemSelectionUI.options[i].GetComponent<SelectorMovement>().controller = owner;
            owner.itemSelectionUI.options[i].GetComponent<TextMeshProUGUI>().text = item.consumable.itemName;
            owner.itemSelectionUI.itemAmountText[i].GetComponent<TextMeshProUGUI>().text = item.amount.ToString();

                       
            SelectorMovement e = owner.itemSelectionUI.options[i].GetComponent<SelectorMovement>();
            e.assignedConsumable = itemList[i].consumable;
            e.UpdateTooltip();
            e.EnableOption();
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

    protected override void OnMouseCancelEvent(object sender, InfoEventArgs<KeyCode> e)
    {
        owner.ChangeState<SelectActionState>();
    }

    //IEnumerator Init()
    //{
    //    Debug.Log("se ha utilizado Potion");
    //    yield return null;
    //    owner.inventory.UseConsumable(0, owner.currentUnit);
    //    owner.ChangeState<SelectActionState>();

    //}


}


