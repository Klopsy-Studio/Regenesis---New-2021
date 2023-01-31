using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipmentBunkhouseButton : UIButtons
{
    public DisplayEquipmentBunkhouse displayEquipmentBunkhouse;
    public EquipmentInventory inventory;
    public int equipmentID;
    private int unitProfileID;
    [SerializeField] Image weaponImage;
    [SerializeField] TextMeshProUGUI weaponName;
  
    public void FillVariables(EquipmentInventory _inventory, int i, DisplayEquipmentBunkhouse _displayEquipmentBunkhouse)
    {
        inventory = _inventory;
        equipmentID = i;
        displayEquipmentBunkhouse = _displayEquipmentBunkhouse;
        weaponImage.sprite = inventory.container[equipmentID].weapon.Sprite;
        weaponName.SetText(inventory.container[equipmentID].weapon.EquipmentName);

    }
    public void SetUnitProfileID(int id)
    {
        unitProfileID = id;
        Debug.Log("unitProfileid: " + unitProfileID);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {

        //GameManager.instance.unitProfilesList[UnitProfileID].unitWeapon = inventory.container[equipmentID].weapon;
      
        inventory.TransferEquipmentToUnit(equipmentID, displayEquipmentBunkhouse, unitProfileID);
        displayEquipmentBunkhouse.UpdateWeaponImage(unitProfileID);
    }

   

}
