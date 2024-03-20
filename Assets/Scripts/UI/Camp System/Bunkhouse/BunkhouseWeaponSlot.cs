using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BunkhouseWeaponSlot : MonoBehaviour, IPointerClickHandler
{
	Weapons weapon;
	BunkhouseManager bunkHouseManager;
	public Image weaponImg;
	public int range;
	public int def;

	public int power;
	public int crit;
	
	public List<string> abilitiesDescription;
	public List<string> abilitiesName;
	
	[SerializeField] Image weaponFrame;
	
	public int unitID;
	
	
	void OnEnable()
	{
		BunkhouseManager.OnWeaponButtonCliked += DeactivateFrame;
	}
	
	void OnDisable()
	{
		BunkhouseManager.OnWeaponButtonCliked -= DeactivateFrame;
	}
	public void UpdateWeaponStats(Weapons weapon, BunkhouseManager bunkHouse, int _unitID)
	{
		unitID = _unitID;
		this.weapon = weapon;
		bunkHouseManager = bunkHouse;
		weaponImg.sprite = weapon.Sprite;
		range = weapon.range;
		def = weapon.Defense;
		power = weapon.Power;
		crit = weapon.CriticalPercentage;
		
		abilitiesDescription.Clear();
		abilitiesName.Clear();
		for (int a = 0; a < weapon.Abilities.Length; a++)
		{
			
			
			var description =weapon.Abilities[a].description;
			var name = weapon.Abilities[a].abilityName;
			abilitiesDescription.Add(description);
			abilitiesName.Add(name);
		  
			
		}
	}
	

	private void DeactivateFrame()
	{
		
		weaponFrame.gameObject.SetActive(false);
		
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		bunkHouseManager.WeaponButtonClicked();
		weaponFrame.gameObject.SetActive(true);
		bunkHouseManager.UpdateNewSelectedHunterInfo(weapon, unitID);
		
		for (int i = 0; i < abilitiesDescription.Count; i++)
		{
			bunkHouseManager.abilityTooltipList[i].SetAbilityTooltip(abilitiesName[i], abilitiesDescription[i]);
		}
		
	}
	
	public void UpdateAbilitiesTooltip()
	{
			for (int i = 0; i < abilitiesDescription.Count; i++)
		{
			bunkHouseManager.abilityTooltipList[i].SetAbilityTooltip(abilitiesName[i], abilitiesDescription[i]);
		}
	}
	// public int WeaponDamage { get; private set; }
	// public int WeaponRange { get; private set; }
	// public int WeaponCritic { get; private set; }
	// public int WeaponDefense { get; private set; }
	// //public int WeaponElementalDefense { get; private set; }

	// ForgeManager forgeManager;
	// public TextMeshProUGUI weaponName;
	// public Image weaponImage;
	// [SerializeField] Image weaponFrame;
	// public WeaponUpgrade WeaponUpgrade { get; private set; }


	// public List<string> abilitiesDescription;
	// public List<string> abilitiesName;


	// private void OnEnable()
	// {
	// 	ForgeManager.OnWeaponButtonCliked += DeactivateFrame;
	// }

	// private void DeactivateFrame()
	// {
	// 	weaponFrame.gameObject.SetActive(false);
	// }
	// private void OnDisable()
	// {
	// 	weaponFrame.gameObject.SetActive(false);
	// 	ForgeManager.OnWeaponButtonCliked -= DeactivateFrame;
	// }

	// public void OnPointerClick(PointerEventData eventData)
	// {

	// 	forgeManager.ButtonClicked();
	// 	forgeManager.weaponPanelInfo.GO.SetActive(true);
	// 	forgeManager.weaponPanelInfo.UpdatePanelInfo(this);
	// 	forgeManager.UpdateMaterialRequiredPanel(WeaponUpgrade);
	// 	forgeManager.SelectCurrentWeaponPanelInfo(this);
	// 	//foreach (var material in WeaponUpgrade.materialsRequired)
	// 	//{
	// 	//    Debug.Log("El material requerido es " + material.monsterMaterial.materialName);
	// 	//}
	 
	// 	for (int i = 0; i < abilitiesDescription.Count; i++)
	// 	{
	// 		forgeManager.abilityTooltipList[i].SetAbilityTooltip(abilitiesName[i], abilitiesDescription[i]);
	// 	}

	// 	weaponFrame.gameObject.SetActive(true);

	// }

	// public void SetWeaponInfo(WeaponUpgrade _weaponUpgrade, ForgeManager _forgeManager)
	// {
	// 	WeaponUpgrade = _weaponUpgrade;
	// 	var weapon = _weaponUpgrade.weapon;
	// 	weaponName.SetText(_weaponUpgrade.itemName);
	// 	weaponImage.sprite = _weaponUpgrade.weapon.Sprite;
	// 	forgeManager = _forgeManager;
	// 	WeaponDamage = weapon.Power;
	// 	WeaponRange = weapon.range;
	// 	WeaponCritic = weapon.criticalPercentage;
	// 	WeaponDefense = weapon.Defense;

	// 	for (int i = 0; i < _weaponUpgrade.weapon.Abilities.Length; i++)
	// 	{
	// 		var description =_weaponUpgrade.weapon.Abilities[i].description;
	// 		var name = _weaponUpgrade.weapon.Abilities[i].abilityName;
	// 		abilitiesDescription.Add(description);
	// 		abilitiesName.Add(name);
		  
			
	// 	}

	// }


   

}
