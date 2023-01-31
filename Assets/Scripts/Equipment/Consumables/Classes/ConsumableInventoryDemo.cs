using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//INVENTORY FOR BATTLE. 
public class ConsumableInventoryDemo : MonoBehaviour
{
    [SerializeField] List<consumableSlot> consumableList;
    public List<consumableSlot> ConsumableList { get { return consumableList; } }
    // Start is called before the first frame update

    public void UseConsumable(int indexItem, Unit targetUnit = null, Tile tileSpawn = null, BattleController battleController = null)
    {
        bool consumableUsed = false;
        var item = consumableList[indexItem].Consumable;
        if(item.ConsumableType == ConsumableType.NormalConsumable)
        {
            consumableUsed = consumableList[indexItem].Consumable.ApplyConsumable(targetUnit);
        }
        else if(item.ConsumableType == ConsumableType.TimelineConsumable)
        {
            consumableUsed = consumableList[indexItem].Consumable.ApplyConsumable(tileSpawn, battleController);
        }
     
      
        if (consumableUsed)
        {
            RemoveConsumable(indexItem);
        }
    }

    public void RemoveConsumable(int i)
    {
        var consumableItem = consumableList[i];
        consumableItem.Count--;
        if (consumableItem.Count == 0)
        {
            consumableList.Remove(consumableItem);
        }
    }
 
}

[System.Serializable]
public class consumableSlot
{
    [SerializeField] Consumables consumable;
    [SerializeField] int count;

    public Consumables Consumable { get { return consumable; } }
    public int Count { get { return count; } set { count = value; } }
}
