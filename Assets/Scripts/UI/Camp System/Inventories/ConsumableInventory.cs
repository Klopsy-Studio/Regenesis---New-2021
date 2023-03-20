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
        bool CheckIfAmountWasAddedToCurrentConsumable = false;
        for (int i = 0; i < consumableContainer.Count; i++)
        {
            if (consumableContainer[i].consumable == _consumable)
            {


         
                if (consumableContainer[i].CheckIfIsMoreThan99Amount(_amount))
                {
                    continue;
                }
                else
                {
                    consumableContainer[i].AddAmount(_amount);
                    CheckIfAmountWasAddedToCurrentConsumable = true;
                    break;
                }
              
            }
        }

        if (!CheckIfAmountWasAddedToCurrentConsumable)
        {
           //Ver si hay un consumable compatible Y que no tenga 99 stacks
            bool hasConsumable = false;
            for (int i = 0; i < consumableContainer.Count; i++)
            {
                var currentConsumableSlot = consumableContainer[i];
                if (currentConsumableSlot.consumable != _consumable)
                {
                    continue;
                }

                if (currentConsumableSlot.amount < 99)
                {
                    int temporalAmount = currentConsumableSlot.amount + _amount;
                    currentConsumableSlot.amount = 99; //maxStack;

                    int amountForNextConsumable = temporalAmount - 99;
                    consumableContainer.Add(new ConsumableSlot(currentConsumableSlot.consumable, amountForNextConsumable));
                    hasConsumable = true;
                    break;
                }
            }

            if (!hasConsumable)
            {
                consumableContainer.Add(new ConsumableSlot(_consumable, _amount));
            }

        }
    }

    public void TransferConsumablesToBackPack(ConsumableInventory targetInventory, int consumableID, DisplayConsumableInventoryBarrack displayconsumableInventory)
    {
        //Check If there is already 4 items
        if (targetInventory.consumableContainer.Count >= 4)
        {
            Debug.Log("there is already 4 items");
            Debug.Log("target inventory tiene" + targetInventory.consumableContainer.Count + "objetos");
            return;
        }

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
