using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Steamworks;


public class ShopManager : MonoBehaviour, IDataPersistence
{
	[Header("tutorialVariable")]
	[SerializeField] GameObject tutorialPanel;
	bool isTutorialFinished = false;

	[SerializeField] ShopItemContainer shopItemContainer;
	[SerializeField] GameObject slotPrefab;
	[SerializeField] Transform transformContent;
	public int currentPoints;
	public Slider slider;

	/* [HideInInspector]*/
	public ShopItemTemplate shopItemSelected;
	public SetShopItemInfoPanelText itemPanelInfo;



	public delegate void ShopButtonSlotClicked();
	public static event ShopButtonSlotClicked OnShopButtonCliked;

	public Animator animator;
	public void ButtonClicked()
	{
		OnShopButtonCliked?.Invoke();
	}

	public void UpdateCurrentPointsAndSlider(int _points, int itemCost)
	{
		int itemQuantity =0;
		Debug.Log("itemCost" + itemCost);
		float result;
		currentPoints += _points;
		
		result = (float)currentPoints / itemCost;
		StartCoroutine(LerpSlider(slider.value, result, 0.3f));
		
		if(currentPoints == itemCost)
		{
			currentPoints -= itemCost;
			itemQuantity++;
		}
		
		Debug.Log("cureent points :" + currentPoints + "itemCost: " + itemCost + "slidervalue: " +slider.value);
	}


	IEnumerator LerpSlider(float startSlider, float endSlider, float overtime)
	{
		float starTime = Time.time;

		while (Time.time < starTime + overtime)
		{
			slider.value = Mathf.Lerp(startSlider, endSlider, (Time.time - starTime) / overtime);
			yield return null;
		}
		
		slider.value = endSlider;
		
		if(slider.value >= slider.maxValue)
		{
			slider.value =0;
		}
	
		
		

	}

	
	public void FinishTutorial() //UnityButtons
	{
		tutorialPanel.SetActive(false);
		isTutorialFinished = true;
	}

	private void Start()
	{
		tutorialPanel.SetActive(false);
		if (!isTutorialFinished) { tutorialPanel.SetActive(true); }

		itemPanelInfo.GO.SetActive(false);

		CreateDisplay();
		// buyItemPanel.SetBuyPanelInfo(itemPanelInfo, this);
		itemPanelInfo.SetUpButtons();
	}
	private void CreateDisplay()
	{
		for (int i = 0; i < shopItemContainer.shopItems.Length; i++)
		{
			var itemShop = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transformContent);
			var itemInfo = shopItemContainer.shopItems[i];
			itemShop.GetComponent<ShopItemTemplate>().SetItemInfo(itemInfo, this);


		}
	}

	// public void IncreaseAmount() //DEPRECATED UnityButton function calls this method
	// {

	// 	itemPanelInfo.IncreaseAmount();
	// }

	// public void DecreaseAmount()//DEPRECATED UnityButton function calls this method
	// {
	// 	itemPanelInfo.DecreaseAmount();
	// }

	// public void OpenBuyItemPanel()//UnityButton function calls this method DEPRECATED
	// {
	// 	if (itemPanelInfo.itemAmount <= 0) return;

	// 	buyItemPanel.SetBuyPanelInfo(itemPanelInfo, this);
	// 	buyItemPanel.SetUpButtons();

	// }


	public void BuyItem()//UnityButton function calls this method
	{

		itemPanelInfo.BuyItem();

	}

	public void LoadData(GameData data)
	{
		isTutorialFinished = data.isShopTutorialFinished;
		currentPoints = data.shopCurrentPoints;
	}

	public void SaveData(GameData data)
	{
		data.isShopTutorialFinished = isTutorialFinished;
		data.shopCurrentPoints = currentPoints;
	}

	public void ReturnAllMaterials()
	{
		itemPanelInfo.button1.ReturnAllMaterials();
		itemPanelInfo.button2.ReturnAllMaterials();
	}

	// public void ResetPanelInfo() //button of closeButton
	// {
	// 	itemPanelInfo.ResetPanelInfo();

	// }

	public void CancelButton() //button of Cancel
	{
		ReturnAllMaterials();

	}
}

[System.Serializable]
public class SetShopItemInfoPanelText
{
	public GameObject GO;

	//[field:SerializeField] public TextMeshProUGUI ItemName { get; private set;}
	[SerializeField] TextMeshProUGUI itemName;
	[SerializeField] TextMeshProUGUI itemDescription;
	[SerializeField] TextMeshProUGUI itemAmountTxT;
	[SerializeField] TextMeshProUGUI itemCostTxT;
	[SerializeField] TextMeshProUGUI itemPointCostTxT;

	[SerializeField] TextMeshProUGUI itemAmountInInventory;

	//public int itemAmount;
	[SerializeField] ShopManager shopManager;

	public int itemCost;

	public ShopItemInfo itemInfo;

	[SerializeField] MaterialInventory materialInventory;
	[SerializeField] MonsterMaterialSlot material1;
	[SerializeField] MonsterMaterialSlot material2;
	public MaterialPointsShopButton button1;
	public MaterialPointsShopButton button2;





	public void SetItemInfo(ShopItemTemplate _shopItem)
	{
		//ItemName.SetText(_shopItem.name);
		//ItemImage.sprite = _shopItem.item.consumable.itemSprite;
		//ItemImage.SetNativeSize();
		
		itemInfo = _shopItem.item;
		itemName.SetText(_shopItem.name);
		itemDescription.SetText(_shopItem.item.consumable.consumableDescription);
		itemPointCostTxT.SetText(_shopItem.item.pointCosts.ToString() + "pts");
		itemCost = _shopItem.item.pointCosts;
	
		CheckNumberOfItemInInventory(_shopItem);

		// ResetPanelInfo();
	}
	
	

	void CheckNumberOfItemInInventory(ShopItemTemplate _shopItem)
	{
		var itemInfo = _shopItem.item.consumable;
		int numberOfItems = 0;
		var consumableInventory = GameManager.instance.consumableInventory;
		var backpackInventory = GameManager.instance.consumableBackpack;
		foreach (var item in consumableInventory.consumableContainer)
		{
			if (itemInfo == item.consumable)
			{
				numberOfItems += item.amount;
			}
		}

		foreach (var item in backpackInventory.consumableContainer)
		{
			if (itemInfo == item.consumable)
			{
				numberOfItems += item.amount;
			}
		}

		itemAmountInInventory.SetText(numberOfItems.ToString());
	}
	// public void IncreaseAmount() //DEPRECATED
	// {
	// 	itemAmount++;
	// 	itemCost = itemAmount * itemInfo.pointCosts;
	// 	itemAmountTxT.SetText("x" + itemAmount);
	// 	itemCostTxT.SetText("Total: " + itemCost + "p");
	// 	if (itemAmount > 0)
	// 	{
	// 		tradeButton.SetActive(true);
	// 	}

	// }

	// public void DecreaseAmount() //DEPRECATED
	// {
	// 	if (itemAmount <= 0) return;
	// 	itemAmount--;
	// 	if (itemAmount == 0) tradeButton.SetActive(false);
	// 	itemCost = itemAmount * itemInfo.pointCosts;
	// 	itemAmountTxT.SetText("x" + itemAmount);
	// 	itemCostTxT.SetText("Total: " + itemCost + "p");
	// }

	//Por ahora no hace falta esta funcion 11/03/2024
	// public void ResetPanelInfo()
	// {
	// 	// itemAmount = 0;
	// 	itemCost = 0;
	// 	// itemAmountTxT.SetText("x" + itemAmount);
	// 	itemCostTxT.SetText("Total: " + itemCost + "p");

	// }

	public void SetUpButtons()
	{

		var item1 = materialInventory.materialContainer.Find(x => x.material == material1.material);
		if (item1 != null)
		{
			material1.amount = item1.amount;
		}

		button1.SetMaterial(material1, this);

		var item2 = materialInventory.materialContainer.Find(y => y.material == material2.material);
		if (item2 != null)
		{
			material2.amount = item2.amount;
		}
		button2.SetMaterial(material2, this);


	}
	public void BuyItem()
	{
		// if (shopManager.currentPoints < itemTotalCost)
		// {
		// 	AudioManager.instance.Play("NoPurchase");
		// 	return;
		// }
		// shopManager.animator.SetTrigger("purchased");
		// AudioManager.instance.Play("ComprarTienda");

		// UpdateCurrentPoints(-itemTotalCost);

		// //REVISAR ESTA LINEA DE CODIGO 04/03/24
		// GameManager.instance.consumableInventory.AddConsumable(itemInfo.consumable, itemAmount);


		// //GameManager.instance.materialInventory.SubstractMaterial(material1);
		// //GameManager.instance.materialInventory.SubstractMaterial(material2);

		// //actualizar los buttons;
		// SetUpButtons();

	}


	//es necesario esta funcion o necesita modificarse? -06/03/24
	public void UpdateCurrentPoints(int _points)
	{
		shopManager.UpdateCurrentPointsAndSlider(_points, itemCost);
		// currentPointsTxTBuyPanel.SetText(shopManager.currentPoints.ToString());

		// UpdateConfirmButtonStatus();

	}


}

// [System.Serializable]
// public class BuyItemPanel
// {



// 	[SerializeField] Sprite itemIMG;
// 	[SerializeField] TextMeshProUGUI itemTotalCostTxT;
// 	[SerializeField] TextMeshProUGUI currentPointsTxTBuyPanel;

// 	public int itemTotalCost;

// 	//int itemAmount;


// 	[SerializeField] MaterialInventory materialInventory;
// 	[SerializeField] MonsterMaterialSlot material1;
// 	[SerializeField] MonsterMaterialSlot material2;
// 	public MaterialPointsShopButton button1;
// 	public MaterialPointsShopButton button2;
// 	ShopItemInfo itemInfo;

// 	public ShopManager shopManager;

// 	public GameObject confirmButton;

// 	public void SetBuyPanelInfo(SetShopItemInfoPanelText _itemInfoPanel, ShopManager _shopManager)
// 	{
// 		confirmButton.SetActive(false);
// 		//itemName.SetText(_itemInfoPanel.ItemName.text);
// 		//itemImage.sprite = _itemInfoPanel.ItemImage.sprite;
// 		itemTotalCost = _itemInfoPanel.itemCost;
// 		itemTotalCostTxT.SetText(itemTotalCost.ToString());
// 		//itemAmount = _itemInfoPanel.itemAmount;
// 		//itemAmountTxT.SetText("x "+_itemInfoPanel.itemAmount.ToString());
// 		shopManager = _shopManager;
// 		//currentPoints = 0;
// 		currentPointsTxTBuyPanel.SetText(shopManager.currentPoints.ToString());

// 		itemInfo = _itemInfoPanel.itemInfo;


// 	}



// 	public void SetUpButtons()
// 	{

// 		var item1 = materialInventory.materialContainer.Find(x => x.material == material1.material);
// 		if (item1 != null)
// 		{
// 			material1.amount = item1.amount;
// 		}

// 		button1.SetMaterial(material1, this);

// 		var item2 = materialInventory.materialContainer.Find(y => y.material == material2.material);
// 		if (item2 != null)
// 		{
// 			material2.amount = item2.amount;
// 		}
// 		button2.SetMaterial(material2, this);


// 	}

// 	public void UpdateCurrentPoints(int _points)
// 	{
// 		shopManager.UpdateCurrentPointsAndSlider(_points, itemTotalCost);
// 		currentPointsTxTBuyPanel.SetText(shopManager.currentPoints.ToString());

// 		UpdateConfirmButtonStatus();

// 	}

// 	public void UpdateConfirmButtonStatus()
// 	{
// 		if (shopManager.currentPoints >= itemTotalCost)
// 		{
// 			confirmButton.SetActive(true);
// 		}
// 		else
// 		{
// 			confirmButton.SetActive(false);
// 		}
// 	}
// 	public void BuyItem()
// 	{
// 		if (shopManager.currentPoints < itemTotalCost)
// 		{
// 			AudioManager.instance.Play("NoPurchase");
// 			return;
// 		}
// 		shopManager.animator.SetTrigger("purchased");
// 		AudioManager.instance.Play("ComprarTienda");

// 		UpdateCurrentPoints(-itemTotalCost);

// 		//REVISAR ESTA LINEA DE CODIGO 04/03/24
// 		//GameManager.instance.consumableInventory.AddConsumable(itemInfo.consumable, itemAmount);


// 		//GameManager.instance.materialInventory.SubstractMaterial(material1);
// 		//GameManager.instance.materialInventory.SubstractMaterial(material2);

// 		//actualizar los buttons;
// 		SetUpButtons();

// 	}

// }
