using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "ShopItemContainer")]
public class ShopItemContainer : ScriptableObject
{
    public ShopItemInfo[] shopItems;
}

[System.Serializable]
public class ShopItemInfo
{
    public string name;
    public Consumables consumable;
    public int pointCosts;
}
