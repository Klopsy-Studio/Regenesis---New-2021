using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsBackpackButton : UIButtons
{

    public DisplayConsumableBackpack displayConsumableBackpack;
    public ConsumableBackpack inventory;
    [HideInInspector] public int consumableID;

    public void FillVariables(ConsumableBackpack _inventory, int i, DisplayConsumableBackpack _displayConsumableBackpack)
    {
        inventory = _inventory;
        consumableID = i;
        displayConsumableBackpack = _displayConsumableBackpack;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        //var inventorySlot = inventory.consumableContainer[consumableID];
        //GameManager.instance.consumableBackpack.AddConsumable(inventorySlot.consumable, inventorySlot.amount);

        var consumableInventory = GameManager.instance.consumableInventory;
        inventory.TransferConsumablesToInventory(consumableInventory, consumableID, displayConsumableBackpack);
        Debug.Log("PIPO");

    }
}
