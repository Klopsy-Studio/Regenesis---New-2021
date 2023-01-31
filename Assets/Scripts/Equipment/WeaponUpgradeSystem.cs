using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponUpgrade")]
public class WeaponUpgradeSystem : ScriptableObject
{

   public AllWeaponsTrees[] allWeaponsTrees;
   
}

[System.Serializable]
public class AllWeaponsTrees
{
    public string TreeName;
    public WeaponUpgrade[] weaponUpgrade;
   
  
}

[System.Serializable]
public class WeaponUpgrade
{
    public string itemName;
    public Weapons weapon;
  
    public Weapons weaponRequired;
    public MaterialRequirement[] materialsRequired;
  
    public bool HasRequiredWeapon(EquipmentInventory _inventory)
    {
        foreach (var i in _inventory.container)
        {
            if(i.weapon == weaponRequired)
            {
                return true;
            }
     
        }

        return false;
    }

    public void QuitRequiredWeapon(EquipmentInventory _inventory)
    {
        if(weaponRequired == null) { return; }
        foreach (var i in _inventory.container)
        {
            if (i.weapon == weaponRequired)
            {
                _inventory.container.Remove(i);
                break;
            }
           
        }
    }
}



[System.Serializable]
public class MaterialRequirement
{
    public MonsterMaterial monsterMaterial;
    public int numberOfMaterial;

    public bool DoIHaveEnoughMaterial(MaterialInventory inventory)
    {
        Debug.Log("hello there");
        foreach (var item in inventory.materialContainer)
        {
            if(item.material == monsterMaterial)
            {
              
               if(item.amount>= numberOfMaterial)
               {
                    return true;
               }
               else
               {
                    return false;
               }

            }
          
        }

        return false;
    }

    public void ReduceMaterial(MaterialInventory inventory)
    {
        foreach (var item in inventory.materialContainer)
        {
            if (item.material == monsterMaterial)
            {

                item.amount -= numberOfMaterial;

            }

        }

    }

}
