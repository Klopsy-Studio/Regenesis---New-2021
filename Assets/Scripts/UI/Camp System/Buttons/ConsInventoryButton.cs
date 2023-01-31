using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsInventoryButton : UIButtons
{
    public DisplayConsumableInventory displayconsumableInventory;
    public ConsumableInventory inventory;
    public int consumableID;
    [SerializeField] Image itemImage;
    [SerializeField] Text itemAmountText;

    public void FillVariables(ConsumableInventory _inventory, int i, DisplayConsumableInventory _displayconsumableInventory)
    {
        inventory = _inventory;
        consumableID = i;
        displayconsumableInventory = _displayconsumableInventory;

        itemImage.sprite = _inventory.consumableContainer[i].consumable.iconSprite;
        itemAmountText.text = _inventory.consumableContainer[i].amount.ToString();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        //var inventorySlot = inventory.consumableContainer[consumableID];
        //GameManager.instance.consumableBackpack.AddConsumable(inventorySlot.consumable, inventorySlot.amount);

        var backpackInventory = GameManager.instance.consumableBackpack;
        inventory.TransferConsumablesToBackPack(backpackInventory, consumableID, displayconsumableInventory);
        
    }
}
