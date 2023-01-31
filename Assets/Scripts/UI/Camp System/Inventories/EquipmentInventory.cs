using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "InventorySystem/EquipmentInventory")]
public class EquipmentInventory : ScriptableObject
{
   
    public List<WeaponSlot> container = new List<WeaponSlot>();
    public void AddItem(Weapons _equipment)
    {
     
        //bool hasEquipment = false;
        //for (int i = 0; i < container.Count; i++)
        //{
        //    if (container[i].weapon == _equipment)
        //    {

        //        hasEquipment = true;
        //        container.Add(new WeaponSlot(_equipment));
        //        break;
        //    }
        //}

        //if (!hasEquipment)
        //{
        //    container.Add(new WeaponSlot(_equipment));
        //}
        container.Add(new WeaponSlot(_equipment));
     
    }

    //private void OnApplicationQuit()
    //{
    //   container.Clear();
    //}


    public void TransferEquipmentToUnit(int equipmentID, DisplayEquipmentBunkhouse displayEquipmentBunkhouse, int unitProfileID)
    {
        //Guardar referencia al viejo arma
        //Sustituir por el neuvo arma
        //sustituir UI de armas
        var weaponSlot = GameManager.instance.equipmentInventory.container[equipmentID];
        var oldWeapon = GameManager.instance.unitProfilesList[unitProfileID].unitWeapon;

        GameManager.instance.unitProfilesList[unitProfileID].unitWeapon = weaponSlot.weapon;
        container.Remove(weaponSlot);
        displayEquipmentBunkhouse.equipmentDisplayed.Remove(weaponSlot);

        displayEquipmentBunkhouse.slotPrefablist[equipmentID].gameObject.SetActive(false);
        displayEquipmentBunkhouse.slotPrefablist.RemoveAt(equipmentID);
        GameManager.instance.equipmentInventory.AddItem(oldWeapon);

    }
}

[System.Serializable]
public class WeaponSlot
{
    public Weapons weapon;
    
    public WeaponSlot(Weapons _weapon)
    {
        weapon = _weapon;
    }

    //public void AddAmount(int value)
    //{
    //    amount += value;
    //}

}

