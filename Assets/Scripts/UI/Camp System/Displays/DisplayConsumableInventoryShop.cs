using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayConsumableInventoryShop : MonoBehaviour
{
    public ConsShopSlot slotPrefab;
    public ConsumableInventory inventory;
   
    [HideInInspector] public Dictionary<ConsumableSlot, ConsShopSlot> consumableDisplayed = new Dictionary<ConsumableSlot, ConsShopSlot>();


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
           
            consumableDisplayed.Add(inventory.consumableContainer[i], obj);
            obj.SetVariables(inventory.consumableContainer[i]);
        }
    }


    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.consumableContainer.Count; i++)
        {
            if (consumableDisplayed.ContainsKey(inventory.consumableContainer[i]))
            {

                consumableDisplayed[inventory.consumableContainer[i]].amountText.SetText(inventory.consumableContainer[i].amount.ToString());
            }
            else
            {
                var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
            
                //obj.consumableImage.sprite = inventory.consuma
                consumableDisplayed.Add(inventory.consumableContainer[i], obj);
                obj.SetVariables(inventory.consumableContainer[i]);


            }

        }
    }
}
