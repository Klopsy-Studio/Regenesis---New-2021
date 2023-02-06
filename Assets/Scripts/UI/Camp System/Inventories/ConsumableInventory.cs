using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "InventorySystem/ConsumableInventory")]
public class ConsumableInventory : ScriptableObject
{
    public List<ConsumableSlot> consumableContainer = new List<ConsumableSlot>();
    public virtual void AddConsumable(Consumables _consumable, int _amount)
    {
        bool hasConsumable = false;
        for (int i = 0; i < consumableContainer.Count; i++)
        {
            if (consumableContainer[i].consumable == _consumable)
            {


                //if (!consumableContainer[i].AddAmount(_amount))
                //{
                //    continue;
                //}
                if (consumableContainer[i].CheckIfIsMoreThan99Amount(_amount))
                {
                    continue;
                }
                else
                {
                    consumableContainer[i].AddAmount(_amount);
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

    public void TransferConsumablesToBackPack(ConsumableInventory targetInventory, int consumableID, DisplayConsumableInventory displayconsumableInventory)
    {
        var inventorySlot = consumableContainer[consumableID];
        var amountToTransfer = inventorySlot.AmountToTransfer();
        if(inventorySlot.amount == 0)
        {
            displayconsumableInventory.consumableDisplayed.Remove(inventorySlot);
            displayconsumableInventory.slotPrefabList[consumableID].gameObject.SetActive(false);
            displayconsumableInventory.slotPrefabList.RemoveAt(consumableID);
            consumableContainer.Remove(inventorySlot);
        }
        //targetInventory.AddConsumable(inventorySlot.consumable, inventorySlotAmount);
        targetInventory.AddConsumable(inventorySlot.consumable, amountToTransfer);
        //displayconsumableInventory.UpdateDisplay();
    }

    //private void OnApplicationQuit()
    //{
    //   container.Clear();
    //}
}

[System.Serializable]
public class ConsumableSlot
{
    public Consumables consumable;
    public int amount;


    public int AmountToTransfer()
    {
        int amountToTransfer;
        if (amount - consumable.maxBackPackAmount < 0)
        {
            amountToTransfer = amount;
            amount = 0;
        }
        else
        {
            amount -= consumable.maxBackPackAmount;
            amountToTransfer = consumable.maxBackPackAmount;
        }


        return amountToTransfer;
        //amount -= 3;
        //return 3;
    }

    public ConsumableSlot (Consumables _consumable, int _amount)
    {
        consumable = _consumable;
        amount = _amount;
    }

    public void AddAmount(int _newAmount)
    {
 
        //-------
        amount += _newAmount;
      
    }

    public bool CheckIfIsMoreThan99Amount(int _newAmount)
    {
        bool isMoreThan99;
        if (amount + _newAmount > 99)
        {
            isMoreThan99 = true; 
        }
        else
        {
            isMoreThan99=false;
        }

        return isMoreThan99;
    }




}
