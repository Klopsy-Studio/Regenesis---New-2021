using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptableObjectsManager : MonoBehaviour, IDataPersistence
{
    //List<ItemData> savedConsumableItems = new List<ItemData>();
    [Header("Consumable Inventory")]
    [SerializeField] List<Consumables> allConsumableItems;
    [SerializeField] ConsumableInventory consumableInventory;
    [SerializeField] ConsumableBackpack consumableBackpack;

    [Header("Material Inventory")]
    [SerializeField] List<MonsterMaterial> allMaterialItems;
    [SerializeField] MaterialInventory materialInventory;

    [Header("Equipment Inventory")]
    [SerializeField] List<Weapons> allWeaponItems;
    [SerializeField] EquipmentInventory weaponInventory;

    [Header("AllMissions")]
    [SerializeField] List<LevelData> allLevelData;

    [Header("Unit profile list")]
    [SerializeField] List<UnitProfile> allUnitProfile;

    public void LoadData(GameData data)
    {

        LoadConsumableInventoryData(data);
        LoadConsumableBackpackData(data);
        LoadMaterialData(data);
        LoadEquipmentData(data);
        LoadMission(data);
        LoadUnitProfile(data);
    }

    void LoadUnitProfile(GameData data)
    {
        for (int i = 0; i < allUnitProfile.Count; i++)
        {
            var unitProfile = allUnitProfile[i];

            
            foreach (var unit in data.unitsProfiles)
            {
                
                if(unitProfile.unitName == unit.Key)
                {
                    Debug.Log("UNIT PROFILE " + unitProfile.unitName + " unit key " + unit.Key);
                    //When we have the unit, we want to add the correct weapon
                    for (int w = 0; w < allWeaponItems.Count; w++)
                    {
                        var weapon = allWeaponItems[w];
                        if(weapon.name == unit.Value)
                        {
                            Debug.Log("weapon name " + weapon.name + " unit key " + unit.Value);
                            unitProfile.unitWeapon = weapon;
                            continue;
                        }
                    }
                }
            }
        }
    }

    private void LoadMission(GameData data)
    {
        for (int i = 0; i < allLevelData.Count; i++)
        {
            var levelData = allLevelData[i];
            data.isMissionNew.TryGetValue(levelData.missionName, out levelData.isOld);
            data.isMissionCompleted.TryGetValue(levelData.missionName, out levelData.hasBeenCompleted);
        }
    
    }

    private void LoadConsumableInventoryData(GameData data)
    {
        consumableInventory.consumableContainer.Clear();
        for (int i = 0; i < data.consumableInventory.Count; i++)
        {
            var itemData = data.consumableInventory[i];

            for (int w = 0; w < allConsumableItems.Count; w++)
            {
                if (itemData.itemId == allConsumableItems[w].itemName)
                {
                    consumableInventory.AddConsumable(allConsumableItems[w], itemData.itemAmount);
                    continue;
                }
            }
        }
    }

    private void LoadConsumableBackpackData(GameData data)
    {
        consumableBackpack.consumableContainer.Clear();
        for (int i = 0; i < data.consumableBackpack.Count; i++)
        {
            var itemData = data.consumableBackpack[i];

            for (int w = 0; w < allConsumableItems.Count; w++)
            {
                if(itemData.itemId == allConsumableItems[w].itemName)
                {
                    consumableBackpack.AddConsumable(allConsumableItems[w], itemData.itemAmount);
                    continue;
                }
            }
        }
    }

    private void LoadMaterialData(GameData data)
    {
        materialInventory.materialContainer.Clear();
        for (int i = 0; i < data.materialInventory.Count; i++)
        {
            var itemData = data.materialInventory[i];
            for (int w = 0; w < allMaterialItems.Count; w++)
            {
                if(itemData.itemId == allMaterialItems[w].materialName)
                {
                    materialInventory.AddMonsterMaterial(allMaterialItems[w], itemData.itemAmount);
                    continue;
                }
            }
        }
    }

    private void LoadEquipmentData(GameData data)
    {
        weaponInventory.container.Clear();
        for (int i = 0; i < data.weaponInventory.Count; i++)
        {
            var itemData = data.weaponInventory[i];
            for (int w = 0; w < allWeaponItems.Count; w++)
            {
                if(itemData.itemId == allWeaponItems[w].name)
                {
                    weaponInventory.AddItem(allWeaponItems[w]);
                    continue;
                }
            }
        }
    }

    public void SaveData(GameData data)
    {
        SaveConsumableInventory(data);
        SaveConsumableBackpack(data);
        SaveMaterialInventory(data);
        SaveWeaponsInventory(data);
        SaveMission(data);
        SaveUnitProfile(data);
    }

    private void SaveConsumableInventory(GameData data)
    {
        data.consumableInventory.Clear();
        //data.consumableItems = new List<ItemData>(new ItemData[inventory.consumableContainer.Count]);
        
        for (int i = 0; i < consumableInventory.consumableContainer.Count; i++)
        {
            data.consumableInventory.Add(new ItemData(consumableInventory.consumableContainer[i].consumable.itemName, consumableInventory.consumableContainer[i].amount));

            //Debug.Log("nombre consumbale items " + consumableInventory.consumableContainer[i].consumable.itemName);
        }
    }

    private void SaveConsumableBackpack(GameData data)
    {
        data.consumableBackpack.Clear();
        for (int i = 0; i < consumableBackpack.consumableContainer.Count; i++)
        {
            Debug.Log("saving consumableBackpack");
            data.consumableBackpack.Add(new ItemData(consumableBackpack.consumableContainer[i].consumable.itemName, consumableBackpack.consumableContainer[i].amount));
        }
    }

    private void SaveMaterialInventory(GameData data)
    {
        data.materialInventory.Clear();
        //Debug.Log("en el material inventory hay: " +materialInventory.materialContainer.Count+"de materiales");

        for (int i = 0; i < materialInventory.materialContainer.Count; i++)
        {
            data.materialInventory.Add(new ItemData(materialInventory.materialContainer[i].material.materialName, materialInventory.materialContainer[i].amount));
            //Debug.Log("nombre material items " + materialInventory.materialContainer[i].material.materialName);
        }
    }

    private void SaveWeaponsInventory(GameData data)
    {
        data.weaponInventory.Clear();
        for (int i = 0; i < weaponInventory.container.Count; i++)
        {
            data.weaponInventory.Add(new ItemData(weaponInventory.container[i].weapon.name, 1));
        }
    }

    void SaveMission(GameData data)
    {
        for (int i = 0; i < allLevelData.Count; i++)
        {
            var levelData = allLevelData[i];
            if (data.isMissionNew.ContainsKey(levelData.missionName))
            {
                data.isMissionNew.Remove(levelData.missionName);
                data.isMissionCompleted.Remove(levelData.missionName);
            }

            data.isMissionNew.Add(levelData.missionName, levelData.isOld);
            data.isMissionCompleted.Add(levelData.missionName, levelData.hasBeenCompleted);
        }
     
    }

    void SaveUnitProfile(GameData data)
    {
        data.unitsProfiles.Clear();
        for (int i = 0; i < allUnitProfile.Count; i++)
        {
            var unit = allUnitProfile[i];


            data.unitsProfiles.Add(unit.unitName, unit.unitWeapon.name);
        }
    }
}
