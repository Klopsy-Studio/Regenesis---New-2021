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
	SetShopItemInfoPanelText itemPanelInfo;

	public MonsterMaterialSlot materialSlot;
	
	public int originalMaterialAmounts;
	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("it is calling");
		if (eventData.button == PointerEventData.InputButton.Left)
		{
		
			if (originalMaterialAmounts <= 0) return;
			
			if (materialSlot.amount <= 0) return;
			
			UseMaterial();
		}   
		// else if (eventData.button == PointerEventData.InputButton.Right)
		// {
		// 	ReturnMaterial();
		// }
		   
	}

	public void SetMaterial(MonsterMaterialSlot _materialSlot, SetShopItemInfoPanelText _itemPanelInfo)
	{
		materialImage.sprite = _materialSlot.material.sprite;
		amountText.SetText(_materialSlot.amount.ToString());
		materialSlot = _materialSlot;
		itemPanelInfo = _itemPanelInfo;
		originalMaterialAmounts = _materialSlot.amount;
	}
	
	
	

	void UseMaterial()
	{

		//if (buyItemPanel.shopManager.currentPoints >= buyItemPanel.itemTotalCost) return;
		materialSlot.amount -= 1;
		amountText.SetText(materialSlot.amount.ToString());
		itemPanelInfo.UpdateCurrentPoints(points);

		//NEW CODE
		GameManager.instance.materialInventory.SubstractMaterial(materialSlot);
	   
		
	}

	void ReturnMaterial()
	{
		if (materialSlot.amount >= originalMaterialAmounts) return;
		materialSlot.amount += 1;
		amountText.SetText(materialSlot.amount.ToString());
		itemPanelInfo.UpdateCurrentPoints(-points);
		GameManager.instance.materialInventory.AddMonsterMaterial(materialSlot.material, 1);
		Debug.Log("return material method");
	}

	public void ReturnAllMaterials()
	{
		while (materialSlot.amount < originalMaterialAmounts)
		{
		 
			materialSlot.amount += 1;
		   
			amountText.SetText(materialSlot.amount.ToString());
			itemPanelInfo.UpdateCurrentPoints(-points);
			GameManager.instance.materialInventory.AddMonsterMaterial(materialSlot.material, 1);
			Debug.Log("return all material method");
		}
		//if (materialSlot.amount >= originalMaterialAmounts) return;
		//int difference = originalMaterialAmounts - materialSlot.amount;
		//int returnPoints = difference * points;
		//materialSlot.amount = originalMaterialAmounts;
		//amountText.SetText(materialSlot.amount.ToString());
		//buyItemPanel.UpdateCurrentPoints(-returnPoints);
		//Debug.Log("aaaaaa");
	}
}
