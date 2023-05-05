using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MaterialSlotButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image materialImage;
    [SerializeField] TextMeshProUGUI amountText;
    public string materialName;
    public string description;
    [SerializeField] Image selectedFrame;
    // Start is called before the first frame update

    MaterialInventoryMananger materialInventoryManager;

    public void SetSlotButton(MonsterMaterialSlot _materialSlot, MaterialInventoryMananger _inventoryManager)
    {
        materialImage.sprite = _materialSlot.material.sprite;
        amountText.SetText(_materialSlot.amount.ToString());
        materialName = _materialSlot.material.materialName;
        description = _materialSlot.material.materialDescription;
        materialInventoryManager = _inventoryManager;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        materialInventoryManager.ButtonClicked();
        materialInventoryManager.UpdateMaterialPanelInfo(this);
        selectedFrame.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        MaterialInventoryMananger.OnMatButtonCliked += DeactiveFrame;
    }

    private void OnDisable()
    {
        DeactiveFrame();
        MaterialInventoryMananger.OnMatButtonCliked -= DeactiveFrame;
    }

    private void DeactiveFrame()
    {
        selectedFrame.gameObject.SetActive(false);
    }
}
