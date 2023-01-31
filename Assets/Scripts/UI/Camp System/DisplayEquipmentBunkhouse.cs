using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;


public class DisplayEquipmentBunkhouse : MonoBehaviour
{
    public GameObject slotPrefab;
    public EquipmentInventory inventory;
    public BunkhouseUnitManager bunkHouseManager;

    int unitProfileID = 0;
    public List<GameObject> slotPrefablist;
    public Dictionary<WeaponSlot, GameObject> equipmentDisplayed = new Dictionary<WeaponSlot, GameObject>();
    void Start()
    {
        BunkhouseUnitManager.changeUnitWeaponID += UpdateUnitsProfileID;
        CreateDisplay();

    }

    public void UpdateWeaponImage(int i)
    {
        bunkHouseManager.UpdateWeaponIMG(i);
        bunkHouseManager.FillUnitVariables(i);
        bunkHouseManager.UpdateDefaultWeaponPanel();

    }

    public void SetUnitProfileID(int id)
    {

        unitProfileID = id;
    }

    private void Update()
    {

        UpdateDisplay();
    }

     void CreateDisplay()
    {
        for (int i = 0; i < inventory.container.Count; i++)
        {
            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
            slotPrefablist.Add(obj);
            //obj.transform.GetChild(1).GetComponentInChildren<Image>().sprite = inventory.container[i].weapon.Sprite;

            equipmentDisplayed.Add(inventory.container[i], obj);
            if (obj.TryGetComponent(out EquipmentBunkhouseButton button))
            {
                button.FillVariables(inventory, i, this);
            }
        }
    }

     void UpdateDisplay()
    {
        
        for (int i = 0; i < inventory.container.Count; i++)
        {

            if (!equipmentDisplayed.ContainsKey(inventory.container[i]))
            {
                var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
                slotPrefablist.Add(obj);
                obj.transform.GetChild(1).GetComponentInChildren<Image>().sprite = inventory.container[i].weapon.Sprite;
                equipmentDisplayed.Add(inventory.container[i], obj);
            }

            if (slotPrefablist[i].TryGetComponent(out EquipmentBunkhouseButton button))
            {
               
                button.FillVariables(inventory, i, this);
            }

        }
    }

    void UpdateUnitsProfileID()
    {
        foreach (var item in slotPrefablist)
        {
            if (item.TryGetComponent(out EquipmentBunkhouseButton button))
            {
                button.SetUnitProfileID(unitProfileID);
            }
        }

    }

    private void OnDisable()
    {
        BunkhouseUnitManager.changeUnitWeaponID -= UpdateUnitsProfileID;
    }


}
