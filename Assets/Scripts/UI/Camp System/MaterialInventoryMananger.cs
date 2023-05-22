using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialInventoryMananger : MonoBehaviour
{
    public MaterialSlotButton slotPrefab;
    public MaterialInventory inventory;

    Dictionary<MonsterMaterialSlot, MaterialSlotButton> materialDisplayed = new Dictionary<MonsterMaterialSlot, MaterialSlotButton>();
    public MaterialPanelInfo materialPanelInfo;
    [SerializeField] Transform contentTransform;

    public delegate void MatButtonSlotClicked();
    public static event MatButtonSlotClicked OnMatButtonCliked;
    public GameObject rightPanel;
    public void ButtonClicked()
    {
        OnMatButtonCliked?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        rightPanel.SetActive(false);
       
    }

    private void OnEnable()
    {
        CreateDisplay();
    }

    //private void Update()
    //{
      
    //    UpdateDisplay();
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    UpdateDisplay();
    //}

    private void CreateDisplay()
    {

        foreach (Transform item in contentTransform)
        {
            Destroy(item.gameObject);
        }

        materialDisplayed.Clear();


        foreach (var item in inventory.materialContainer)
        {
            if (item.amount == 0)
            {
                inventory.materialContainer.Remove(item);
            }
        }

        for (int i = 0; i < inventory.materialContainer.Count; i++)
        {
            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, contentTransform);
            var materialSlot = inventory.materialContainer[i];
            //obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.materialContainer[i].material.sprite;

            obj.SetSlotButton(materialSlot, this);
            materialDisplayed.Add(inventory.materialContainer[i], obj);
        }


     
    }


    private void UpdateDisplay()
    {
        for (int i = 0; i < inventory.materialContainer.Count; i++)
        {
            if (materialDisplayed.ContainsKey(inventory.materialContainer[i]))
            {

                //materialDisplayed[inventory.materialContainer[i]].GetComponentInChildren<Text>().text = inventory.materialContainer[i].amount.ToString();
                materialDisplayed[inventory.materialContainer[i]].amountText.SetText(inventory.materialContainer[i].amount.ToString());
            }
            else
            {
                var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
                var materialSlot = inventory.materialContainer[i];
                obj.SetSlotButton(materialSlot, this);
                materialDisplayed.Add(inventory.materialContainer[i], obj);
            }

        }
    }

    public void UpdateMaterialPanelInfo(MaterialSlotButton _materialSlotButton)
    {
        materialPanelInfo.UpdatePanelInfo(_materialSlotButton);
    }

    private void OnDisable()
    {
        materialPanelInfo.ResetInfo();
        rightPanel.SetActive(false);
    }
}

[System.Serializable]
public class MaterialPanelInfo
{
    public TextMeshProUGUI materialname;
    public TextMeshProUGUI materialDescription;

    public void UpdatePanelInfo(MaterialSlotButton _matSlotButton)
    {
       
        materialname.SetText(_matSlotButton.materialName);
        materialDescription.SetText(_matSlotButton.description);
    }

    public void ResetInfo()
    {
        materialname.SetText("");
        materialDescription.SetText("");
    }
}
