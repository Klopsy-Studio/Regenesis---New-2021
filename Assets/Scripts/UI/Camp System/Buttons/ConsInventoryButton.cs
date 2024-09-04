using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ConsInventoryButton : UIButtons
{
    public DisplayConsumableInventoryBarrack displayconsumableInventory;
    public ConsumableInventory inventory;
    public int consumableID;
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemAmountText;

    public void FillVariables(ConsumableInventory _inventory, int i, DisplayConsumableInventoryBarrack _displayconsumableInventory)
    {
        inventory = _inventory;
        consumableID = i;
        displayconsumableInventory = _displayconsumableInventory;

        itemImage.sprite = _inventory.consumableContainer[i].consumable.iconSprite;
        itemAmountText.text = _inventory.consumableContainer[i].amount.ToString();

        GetComponent<ToolTipTrigger>().header = _inventory.consumableContainer[i].consumable.itemName;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        //var inventorySlot = inventory.consumableContainer[consumableID];
        //GameManager.instance.consumableBackpack.AddConsumable(inventorySlot.consumable, inventorySlot.amount);
      
        var backpackInventory = GameManager.instance.consumableBackpack;
        inventory.TransferConsumablesToBackPack(backpackInventory, consumableID, displayconsumableInventory);
        AudioManager.instance.PlayWithRandomPitch("Backpack", 0.8f, 1.2f);
        
    }
}
