using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayConsumableInventoryBarrack : MonoBehaviour
{
    
    public GameObject slotPrefab;
    public ConsumableInventory inventory;
    public List<GameObject> slotPrefabList;
    [HideInInspector] public Dictionary<ConsumableSlot, GameObject> consumableDisplayed = new Dictionary<ConsumableSlot, GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }


    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    private void CreateDisplay()
    {
        for (int i = 0; i < inventory.consumableContainer.Count; i++)
        {
            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
            slotPrefabList.Add(obj);
            
            consumableDisplayed.Add(inventory.consumableContainer[i], obj);
            if(obj.TryGetComponent(out ConsInventoryButton inventoryButton))
            {
                
                inventoryButton.FillVariables(inventory, i, this);
            }
           
        }
    }


    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.consumableContainer.Count; i++)
        {
            if (consumableDisplayed.ContainsKey(inventory.consumableContainer[i]))
            {

                consumableDisplayed[inventory.consumableContainer[i]].GetComponentInChildren<Text>().text =inventory.consumableContainer[i].amount.ToString();
            }
            else
            {
                var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
                slotPrefabList.Add(obj);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.consumableContainer[i].consumable.iconSprite;
                obj.GetComponentInChildren<Text>().text = inventory.consumableContainer[i].amount.ToString();
                consumableDisplayed.Add(inventory.consumableContainer[i], obj);
           
            }

            if (slotPrefabList[i].TryGetComponent(out ConsInventoryButton inventoryButton))
            {
                
                inventoryButton.FillVariables(inventory, i, this);
            }



        }
    }
}


