using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string itemId;
    public int itemAmount;

  
    public ItemData(string itemId, int itemAmount)
    {
        this.itemId = itemId;
        this.itemAmount = itemAmount;
    }

    public ItemData(string itemId)
    {
        this.itemId = itemId;
    }
}
