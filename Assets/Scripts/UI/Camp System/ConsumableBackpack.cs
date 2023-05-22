using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "InventorySystem/ConsumableBackpack")]
public class ConsumableBackpack : ConsumableInventory
{

    [SerializeField] bool setOriginalConsumables;
    public List<ConsumableSlot> originalConsumables;
    public void UseConsumable(int indexItem, Unit targetUnit = null, Tile tileSpawn = null, BattleController battleController = null)
    {
        bool consumableUsed = false;
        var item = consumableContainer[indexItem].consumable;
        if (item.ConsumableType == ConsumableType.NormalConsumable)
        {
            consumableUsed = consumableContainer[indexItem].consumable.ApplyConsumable(targetUnit);
        }
        else if (item.ConsumableType == ConsumableType.TimelineConsumable)
        {
            consumableUsed = consumableContainer[indexItem].consumable.ApplyConsumable(tileSpawn, battleController);
        }
        else if(item.ConsumableType == ConsumableType.TargetConsumable)
        {
            consumableUsed = consumableContainer[indexItem].consumable.ApplyConsumable(tileSpawn, battleController);
        }
        if (consumableUsed)
        {
            RemoveConsumable(indexItem);
        }
    }

    public void RemoveConsumable(int i)
    {
        var consumableItem = consumableContainer[i];
        consumableItem.amount--;
        if (consumableItem.amount == 0)
        {
            consumableContainer.Remove(consumableItem);
        }
    }

    public override void AddConsumable(Consumables _consumable, int _amount)
    {
        bool hasConsumable = false;
        for (int i = 0; i < consumableContainer.Count; i++)
        {
            if (consumableContainer[i].consumable == _consumable)
            {
                if(consumableContainer[i].amount < _consumable.maxBackPackAmount)
                {
                    var addAmount = _consumable.maxBackPackAmount - consumableContainer[i].amount;

                    consumableContainer[i].AddAmount(addAmount);
                    hasConsumable = true;
                    break;
                }
              
            }
        }

        if (!hasConsumable)
        {
            consumableContainer.Add(new ConsumableSlot(_consumable, _amount));
        }
    }

    public void TransferConsumablesToInventory(ConsumableInventory targetInventory, int consumableID, DisplayConsumableBackpack displayConsumableBackpack)
    {
        var inventorySlot = consumableContainer[consumableID];

        displayConsumableBackpack.consumableDisplayed.Remove(inventorySlot);
        //displayConsumableBackpack.slotPrefabList[consumableID].gameObject.SetActive(false);
        displayConsumableBackpack.slotPrefabList.RemoveAt(consumableID);
        consumableContainer.Remove(inventorySlot);

       

        targetInventory.AddConsumable(inventorySlot.consumable, inventorySlot.amount);
        //displayConsumableBackpack.UpdateDisplay();
    }


    public void OnDisable()
    {
        //consumableContainer.Clear();
    }


    public void ResetConsumables()
    {
        if (setOriginalConsumables)
        {
            consumableContainer.Clear();
            foreach (ConsumableSlot slot in originalConsumables)
            {
                ConsumableSlot s = new ConsumableSlot(slot.consumable, slot.amount);
                consumableContainer.Add(s);
            }
        }
    }
    public void OnEnable()
    {
        if (setOriginalConsumables)
        {
            consumableContainer.Clear();
            foreach (ConsumableSlot slot in originalConsumables)
            {
                ConsumableSlot s = new ConsumableSlot(slot.consumable, slot.amount);
                consumableContainer.Add(s);
            }
        }
    }
}
