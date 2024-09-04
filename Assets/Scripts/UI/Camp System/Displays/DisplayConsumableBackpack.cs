using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DisplayConsumableBackpack : MonoBehaviour
{
    
    public GameObject slotPrefab;
    public ConsumableBackpack inventory;
    public List<GameObject> slotPrefabList;
    [HideInInspector] public Dictionary<ConsumableSlot, GameObject> consumableDisplayed = new Dictionary<ConsumableSlot, GameObject>();

    // Start is called before the first frame update
    public bool isForBox;


    private void OnEnable()
    {
        CreateDisplay();
    }
    void Update()
    {
        if (isForBox)
        {
            UpdateDisplay();
        }

    }

    public void CreateDisplay()
    {

        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        slotPrefabList.Clear();
        consumableDisplayed.Clear();

        for (int i = 0; i < inventory.consumableContainer.Count; i++)
        {
            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
            slotPrefabList.Add(obj);
            obj.transform.GetChild(1).GetComponentInChildren<Image>().sprite = inventory.consumableContainer[i].consumable.iconSprite;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.consumableContainer[i].amount.ToString();
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

                consumableDisplayed[inventory.consumableContainer[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.consumableContainer[i].amount.ToString();
            }
            else
            {
                var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
                slotPrefabList.Add(obj);
                obj.transform.GetChild(1).GetComponentInChildren<Image>().sprite = inventory.consumableContainer[i].consumable.iconSprite;
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.consumableContainer[i].amount.ToString();
                consumableDisplayed.Add(inventory.consumableContainer[i], obj);
            }
            //if (slotPrefabList[i].TryGetComponent(out ConsBackpackButton inventoryButton))
            //{

            //    inventoryButton.FillVariables(inventory, i, this);
            //}

            if (slotPrefabList[i].GetComponent<ConsBackpackButton>() != null)
            {
                slotPrefabList[i].GetComponent<ConsBackpackButton>().FillVariables(inventory, i, this);

            }
        }
    }
}
