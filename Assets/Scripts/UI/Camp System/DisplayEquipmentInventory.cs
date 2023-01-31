using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayEquipmentInventory : MonoBehaviour
{
    public Weapons a;
    public Weapons b;           
    public Weapons c;

    public void AddConsumables()
    {
        inventory.AddItem(a);
        inventory.AddItem(b);
        inventory.AddItem(c);
    }

    public GameObject slotPrefab;
    public EquipmentInventory inventory;

    Dictionary<WeaponSlot, GameObject> equipmentDisplayed = new Dictionary<WeaponSlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("LLAMADA DE INVENTORY");
        CreateDisplay();
    }


    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    void CreateDisplay()
    {
        
        
        for (int i = 0; i < inventory.container.Count; i++)
        {
            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.container[i].weapon.Sprite;
            equipmentDisplayed.Add(inventory.container[i], obj);
          
        }
    }


     void UpdateDisplay()
    {
      
        for (int i = 0; i < inventory.container.Count; i++)
        {
            if (equipmentDisplayed.ContainsKey(inventory.container[i]))
            {

                continue;
            }
            else
            {
               
                var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.container[i].weapon.Sprite;
                equipmentDisplayed.Add(inventory.container[i], obj);
              
            }
          
         
        }



        //for (int i = 0; i < inventory.container.Count; i++)
        //{
        //    if (equipmentDisplayed.ContainsKey(inventory.container[i]))
        //    {

        //        equipmentDisplayed[inventory.container[i]].GetComponentInChildren<Text>().text = inventory.container[i].amount.ToString();
        //    }
        //    else
        //    {
        //        var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
        //        obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.consumableContainer[i].consumable.sprite;

        //        equipmentDisplayed.Add(inventory.consumableContainer[i], obj);
        //    }

        //}
    }
}
