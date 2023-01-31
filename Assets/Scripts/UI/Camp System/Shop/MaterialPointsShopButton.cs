using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MaterialPointsShopButton : MonoBehaviour, IPointerClickHandler
{
    public Image materialImage;
    public TextMeshProUGUI amountText;
    public int points;


    public MonsterMaterialSlot materialSlot;
    BuyItemPanel buyItemPanel;
    public int originalMaterialAmounts;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UseMaterial();
        }   
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ReturnMaterial();
        }
           
    }

    public void SetMaterial(MonsterMaterialSlot _materialSlot, BuyItemPanel _buyItemPanel)
    {
        materialImage.sprite = _materialSlot.material.sprite;
        amountText.SetText(_materialSlot.amount.ToString());
        materialSlot = _materialSlot;
        buyItemPanel = _buyItemPanel;
        originalMaterialAmounts = _materialSlot.amount;
    }

    void UseMaterial()
    {
        materialSlot.amount -= 1;
        amountText.SetText(materialSlot.amount.ToString());
        buyItemPanel.UpdateCurrentPoints(points);
    }

    void ReturnMaterial()
    {
        if (materialSlot.amount >= originalMaterialAmounts) return;
        materialSlot.amount += 1;
        amountText.SetText(materialSlot.amount.ToString());
        buyItemPanel.UpdateCurrentPoints(-points);
        Debug.Log("return material method");
    }
}
