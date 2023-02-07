using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ConsumableInventoryManager : MonoBehaviour
{
    public ConsSlotButton slotPrefab;
    public ConsumableInventory inventory;
    public List<ConsSlotButton> slotPrefabList;
    [HideInInspector] public Dictionary<ConsumableSlot, ConsSlotButton> consumableDisplayed = new Dictionary<ConsumableSlot, ConsSlotButton>();

    public ConsumablePanelInfo consumablePanelInfo;
  
    [SerializeField] Transform contentTransform;
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }



    void Update()
    {
        UpdateDisplay();
    }

    private void CreateDisplay()
    {
        for (int i = 0; i < inventory.consumableContainer.Count; i++)
        {

            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, contentTransform);
            slotPrefabList.Add(obj);
            var consumableSlot = inventory.consumableContainer[i];
            obj.SetSlotButton(consumableSlot, this);
            consumableDisplayed.Add(inventory.consumableContainer[i], obj);
       
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
                var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, contentTransform);
                slotPrefabList.Add(obj);
                var consumableSlot = inventory.consumableContainer[i];
                obj.SetSlotButton(consumableSlot, this);
                consumableDisplayed.Add(inventory.consumableContainer[i], obj);
                //if (obj.TryGetComponent(out ConsInventoryButton inventoryButton))
                //{

                //    inventoryButton.FillVariables(inventory, i, this);
                //}
            }

            //if (slotPrefabList[i].TryGetComponent(out ConsSlotButton slotButton))
            //{

            //    slotButton.FillVariables(this);
            //}

            

        }
    }

    public void UpdateConsPanelInfo(ConsSlotButton _consSlotButton)
    {
        consumablePanelInfo.UpdatePanelInfo(_consSlotButton);
    }

    public void OnDisable()
    {
        consumablePanelInfo.ResetInfo();
    }
}

[System.Serializable]
public class ConsumablePanelInfo
{
    public TextMeshProUGUI consumableName;
    public TextMeshProUGUI consumableDescription;

    public void UpdatePanelInfo(ConsSlotButton _consSlotButton)
    {
        Debug.Log("el nombre es " + _consSlotButton.consName);
        consumableName.SetText(_consSlotButton.consName);
        consumableDescription.SetText(_consSlotButton.description);
    }

    public void ResetInfo()
    {
        consumableName.SetText("");
        consumableDescription.SetText("");
    }
}
