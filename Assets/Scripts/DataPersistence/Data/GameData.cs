using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int clickCount;
    public SerializableDictionary<string, bool> coinsCollected;
    public List<ItemData> consumableInventory;
    public List<ItemData> materialInventory;
    public List<ItemData> weaponInventory;
    public List<ItemData> consumableBackpack; 
    public SerializableDictionary<string, bool> isMissionNew;
    public SerializableDictionary<string, bool> isMissionCompleted;
    //key -> unit name --- value -> unitWeapons
    public SerializableDictionary<string, string> unitsProfiles;

    string[] unitsName = { "Isak", "Kaeo", "Ola" };
    string[] unitsWeapons = { "ScrapBow_3", "ScrapGunBlade_3", "ScrapHammer_3" };
    //the values defined in this constructor will be the default values
    //the game starts with when there's no data to load
    public GameData()
    {
        this.clickCount = 0;
        coinsCollected = new SerializableDictionary<string, bool>();
        consumableInventory = new List<ItemData>();
        materialInventory = new List<ItemData>();
        weaponInventory = new List<ItemData>();
        isMissionNew = new SerializableDictionary<string, bool>();
        isMissionCompleted = new SerializableDictionary<string, bool>();

        //AL INICIAR QUE OBTENGAN ARMAS POR DEFECTOS
        unitsProfiles = new SerializableDictionary<string, string>();
        UnitsProfileDefaultSetting();
        //unitsProfiles = new SerializableDictionary<string, string>();
        consumableBackpack = new List<ItemData>();
    }

    private void UnitsProfileDefaultSetting()
    {
        for (int i = 0; i < 3; i++)
        {
            var name = unitsName[i];
            var weapon = unitsWeapons[i];
            unitsProfiles.Add(name, weapon);
        }
    }
}
