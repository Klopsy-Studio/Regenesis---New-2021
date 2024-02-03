using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class ForgeManager : MonoBehaviour, IDataPersistence
{
	[Header("tutorialVariable")]
	[SerializeField] GameObject tutorialPanel;
	bool isTutorialFinished = false;

	[SerializeField] WeaponUpgradeSystem weaponUpgradeSystem;
	[SerializeField] Transform transformContent;
	[SerializeField] GameObject treePrefab;
	[SerializeField] GameObject weaponSlotPrefab;

	public WeaponPanelInfo weaponPanelInfo;

	[SerializeField] MaterialRequiredSlot[] materialsRequiredSlot;

	//Ponerlo en privado cuando se termine el testeo
	public WeaponInfoTemplate currentWeaponInfoTemplate;
	public AbilityTooltip[] abilityTooltipList;

	public delegate void WeaponButtonSlotClicked();
	public static event WeaponButtonSlotClicked OnWeaponButtonCliked;
	
	[SerializeField] GameObject forgeButton;
	[SerializeField] TextMeshProUGUI selectWeaponTxT;
 	private void Start()
	{
		tutorialPanel.SetActive(false);
		if (!isTutorialFinished) { tutorialPanel.SetActive(true); }
		CreateDisplay();
		weaponPanelInfo.GO.SetActive(false);
		
	}

	private void OnEnable()
	{
		weaponPanelInfo.GO.SetActive(false);
		foreach (var material in materialsRequiredSlot)
		{
			material.gameObject.SetActive(false);
		
		}
		forgeButton.SetActive(false);
		selectWeaponTxT.gameObject.SetActive(true);
	}

	public void FinishTutorial() //UnityButtons
	{
		tutorialPanel.SetActive(false);
		isTutorialFinished = true;
	}

	public void ButtonClicked()
	{
		selectWeaponTxT.gameObject.SetActive(false);
		OnWeaponButtonCliked?.Invoke();
		forgeButton.SetActive(true);
	}

	void CreateDisplay()
	{
		//The first FOR is used to create each weapon tree
		for (int i = 0; i < weaponUpgradeSystem.allWeaponsTrees.Length; i++)
		{
			var weaponTree = Instantiate(treePrefab, Vector3.zero, Quaternion.identity, transformContent);
			var weaponTreeTemplate = weaponTree.GetComponent<WeaponTreeTemplate>();
			weaponTreeTemplate.treeNameText.SetText(weaponUpgradeSystem.allWeaponsTrees[i].TreeName);

			//The seconf FOR is used to create the weapon slot inside of the weapon tree
			for (int w = 0; w < weaponUpgradeSystem.allWeaponsTrees[i].weaponUpgrade.Length; w++)
			{
				var weaponSlot = Instantiate(weaponSlotPrefab, Vector3.zero, Quaternion.identity, weaponTreeTemplate.contentTransform);
				//weaponSlot.GetComponent<WeaponInfoTemplate>().weaponName.SetText(weaponUpgradeSystem.allWeaponsTrees[i].weaponUpgrade[w].itemName);
				weaponSlot.GetComponent<WeaponInfoTemplate>().SetWeaponInfo(weaponUpgradeSystem.allWeaponsTrees[i].weaponUpgrade[w], this);
			}
		}
	}

	public void UpdateMaterialRequiredPanel(WeaponUpgrade _weaponUpgrade)
	{
		//Deactivate all materialSlot
		foreach (var material in materialsRequiredSlot)
		{
			material.gameObject.SetActive(false);
		}
		for (int i = 0; i < _weaponUpgrade.materialsRequired.Length; i++)
		{
			//Activate the materialSlot if is needed
			materialsRequiredSlot[i].gameObject.SetActive(true);
			var materialRequired = _weaponUpgrade.materialsRequired[i];
			
			materialsRequiredSlot[i].SetUpMaterialRequiredSlot(materialRequired);
		}
	}

	public void SelectCurrentWeaponPanelInfo(WeaponInfoTemplate _weaponInfoTemplate)
	{
		currentWeaponInfoTemplate = _weaponInfoTemplate;
	}

	public bool CanPurchaseWeapon()
	{
		//Check if enough materials

		//var materialsRequired = currentWeaponInfoTemplate

	
		for (int i = 0; i < currentWeaponInfoTemplate.WeaponUpgrade.materialsRequired.Length; i++)
		{
	
			if (!currentWeaponInfoTemplate.WeaponUpgrade.materialsRequired[i].DoIHaveEnoughMaterial(GameManager.instance.materialInventory))
			{
		   
				Debug.Log("no tienes suficiente materiales");
				return false;
			}
		}

		//check if you have the required weapon
		if(currentWeaponInfoTemplate.WeaponUpgrade.weaponRequired != null)
		{
			if (!currentWeaponInfoTemplate.WeaponUpgrade.HasRequiredWeapon(GameManager.instance.equipmentInventory))
			{
				Debug.Log("NO TIENES EL ARMA REQUERIDA");
				return false;
			}
		}

		return true;
	}

	public void ReduceMaterialAndWeapon()
	{
		for (int i = 0; i < currentWeaponInfoTemplate.WeaponUpgrade.materialsRequired.Length; i++)
		{
			currentWeaponInfoTemplate.WeaponUpgrade.materialsRequired[i].ReduceMaterial(GameManager.instance.materialInventory);
		}

		currentWeaponInfoTemplate.WeaponUpgrade.QuitRequiredWeapon(GameManager.instance.equipmentInventory);
		
	}

	public void LoadData(GameData data)
	{
		isTutorialFinished = data.isForgeTutorialFinished;
	}

	public void SaveData(GameData data)
	{
	   data.isForgeTutorialFinished = isTutorialFinished;
	}
}

[System.Serializable]
public class WeaponPanelInfo
{
	public GameObject GO;
	public TextMeshProUGUI weaponDamage;
	public TextMeshProUGUI weaponRange;
	public TextMeshProUGUI weaponCritic;
	public TextMeshProUGUI weaponDefense;
  
	public TextMeshProUGUI weaponName;
	public void UpdatePanelInfo(WeaponInfoTemplate _weaponInfoTemplate)
	{
		weaponDamage.SetText(_weaponInfoTemplate.WeaponDamage.ToString());
		weaponRange.SetText(_weaponInfoTemplate.WeaponRange.ToString());
		weaponCritic.SetText(_weaponInfoTemplate.WeaponCritic.ToString()+"%");
		weaponDefense.SetText(_weaponInfoTemplate.WeaponDefense.ToString());
	}

	public void UpdateInventoryPanelInfo(EquipmentSlotButton slotButton)
	{
		weaponName.SetText(slotButton.weaponName);
		weaponDamage.SetText(slotButton.WeaponDamage.ToString());
		weaponRange.SetText(slotButton.WeaponRange.ToString());
		weaponCritic.SetText(slotButton.WeaponCritic.ToString() + "%");
		weaponDefense.SetText(slotButton.WeaponDefense.ToString());
	}

	public void ResetInfo() 
	{
		weaponName.SetText("Name");
		weaponDamage.SetText("Damage: ");
		weaponRange.SetText("Range: ");
		weaponCritic.SetText("Critical: ");
		weaponDefense.SetText("Defense: ");
	}
}
