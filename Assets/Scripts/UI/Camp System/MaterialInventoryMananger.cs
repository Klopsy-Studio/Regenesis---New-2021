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

    public void ButtonClicked()
    {
        OnMatButtonCliked?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }


    // Update is called once per frame
    //void Update()
    //{
    //    UpdateDisplay();
    //}

    private void CreateDisplay()
    {
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

                materialDisplayed[inventory.materialContainer[i]].GetComponentInChildren<Text>().text = inventory.materialContainer[i].amount.ToString();
            }
            else
            {
                var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.materialContainer[i].material.sprite;
                obj.GetComponentInChildren<Text>().text = inventory.materialContainer[i].amount.ToString();
                materialDisplayed.Add(inventory.materialContainer[i], obj);
            }

        }
    }

    public void UpdateMaterialPanelInfo(MaterialSlotButton _materialSlotButton)
    {
        materialPanelInfo.UpdatePanelInfo(_materialSlotButton);
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
