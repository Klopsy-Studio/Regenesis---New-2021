using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int clickCount;
    public NewSerializableDictionary<string, bool> coinsCollected;
    public List<ItemData> consumableInventory;
    public List<ItemData> materialInventory;
    public List<ItemData> weaponInventory;
    public List<ItemData> consumableBackpack; 
    public NewSerializableDictionary<string, bool> isMissionNew;
    public NewSerializableDictionary<string, bool> isMissionCompleted;
    //key -> unit name --- value -> unitWeapons
    public NewSerializableDictionary<string, string> unitsProfiles;
    public int shopCurrentPoints;

    string[] unitsName = { "Isak", "Kaeo", "Ola" };
    string[] unitsWeapons = { "ScrapBow_3", "ScrapGunBlade_3", "ScrapHammer_3" };

    //Tutorial saves
    public bool isBarrackTutorialFinished;
    public bool isForgeTutorialFinished;
    public bool isShopTutorialFinished;
    public bool isInventoryTutorialFinished;
    public bool isMapTutorialFinished;


    //the values defined in this constructor will be the default values
    //the game starts with when there's no data to load
    public GameData()
    {
        this.clickCount = 0;
        coinsCollected = new NewSerializableDictionary<string, bool>();
    
        consumableInventory = new List<ItemData>();

        materialInventory = new List<ItemData>();
 
        materialInventory.Clear();
        
     
        weaponInventory = new List<ItemData>();
        isMissionNew = new NewSerializableDictionary<string, bool>();
        isMissionCompleted = new NewSerializableDictionary<string, bool>();

        //AL INICIAR QUE OBTENGAN ARMAS POR DEFECTOS
        unitsProfiles = new NewSerializableDictionary<string, string>();
        UnitsProfileDefaultSetting();
        //unitsProfiles = new SerializableDictionary<string, string>();
        consumableBackpack = new List<ItemData>();
        shopCurrentPoints = 0;


        isBarrackTutorialFinished = false; 
        isForgeTutorialFinished = false ;
        isShopTutorialFinished = false ;
        isInventoryTutorialFinished=false;
        isMapTutorialFinished = false;

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
