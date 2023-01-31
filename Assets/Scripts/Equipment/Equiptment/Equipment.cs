using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KitType
{
    Hammer,
    Bow,
    Gunblade
}


public abstract class Equipment : ScriptableObject
{
   
    [SerializeField] private string equipmentName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private KitType equipmentType;

    public string EquipmentName { get { return equipmentName; } }
    public Sprite Sprite { get { return sprite; } }
    public KitType EquipmentType { get {return equipmentType;}}

    public abstract void EquipItem(PlayerUnit c);
  
}
