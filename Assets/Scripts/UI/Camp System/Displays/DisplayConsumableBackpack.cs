using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayConsumableBackpack : MonoBehaviour
{
    //public Consumables a;
    //public Consumables b;
    //public Consumables c;

    //public void AddConsumables()
    //{
    //    inventory.AddConsumable(a, 2);
    //    inventory.AddConsumable(b, 1);
    //    inventory.AddConsumable(c, 3);
    //}
    public GameObject slotPrefab;
    public ConsumableBackpack inventory;
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
            obj.transform.GetChild(1).GetComponentInChildren<Image>().sprite = inventory.consumableContainer[i].consumable.iconSprite;
            obj.GetComponentInChildren<Text>().text = inventory.consumableContainer[i].amount.ToString();
            consumableDisplayed.Add(inventory.consumableContainer[i], obj);
            if (obj.TryGetComponent(out ConsBackpackButton inventoryButton))
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

                consumableDisplayed[inventory.consumableContainer[i]].GetComponentInChildren<Text>().text = inventory.consumableContainer[i].amount.ToString();
            }
            else
            {
                var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
                slotPrefabList.Add(obj);
                obj.transform.GetChild(1).GetComponentInChildren<Image>().sprite = inventory.consumableContainer[i].consumable.iconSprite;
                obj.GetComponentInChildren<Text>().text = inventory.consumableContainer[i].amount.ToString();
                consumableDisplayed.Add(inventory.consumableContainer[i], obj);
            }
            if (slotPrefabList[i].TryGetComponent(out ConsBackpackButton inventoryButton))
            {

                inventoryButton.FillVariables(inventory, i, this);
            }
        }
    }
}
