using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;
public class WeaponInfoTemplate : MonoBehaviour, IPointerClickHandler
{
	public int WeaponDamage { get; private set; }
	public int WeaponRange { get; private set; }
	public int WeaponCritic { get; private set; }
	public int WeaponDefense { get; private set; }
	//public int WeaponElementalDefense { get; private set; }

	ForgeManager forgeManager;
	public TextMeshProUGUI weaponName;
	public Image weaponImage;
	[SerializeField] Image weaponFrame;
	public WeaponUpgrade WeaponUpgrade { get; private set; }


	public List<string> abilitiesDescription;
	public List<string> abilitiesName;
	
	public Image chestSprite;

	 Weapons weapon;

	private void OnEnable()
	{
		ForgeManager.OnWeaponButtonCliked += DeactivateFrame;
	}

	private void DeactivateFrame()
	{
		weaponFrame.gameObject.SetActive(false);
	}
	private void OnDisable()
	{
		weaponFrame.gameObject.SetActive(false);
		ForgeManager.OnWeaponButtonCliked -= DeactivateFrame;
	}

	public void OnPointerClick(PointerEventData eventData)
	{

		forgeManager.ButtonClicked();
		forgeManager.weaponPanelInfo.GO.SetActive(true);
		forgeManager.weaponPanelInfo.UpdatePanelInfo(this);
		forgeManager.UpdateMaterialRequiredPanel(WeaponUpgrade);
		forgeManager.SelectCurrentWeaponPanelInfo(this);
		//foreach (var material in WeaponUpgrade.materialsRequired)
		//{
		//    Debug.Log("El material requerido es " + material.monsterMaterial.materialName);
		//}
	 
		for (int i = 0; i < abilitiesDescription.Count; i++)
		{
			forgeManager.abilityTooltipList[i].SetAbilityTooltip(abilitiesName[i], abilitiesDescription[i]);
		}

		weaponFrame.gameObject.SetActive(true);

	}

	public void SetWeaponInfo(WeaponUpgrade _weaponUpgrade, ForgeManager _forgeManager)
	{
		WeaponUpgrade = _weaponUpgrade;
		weapon = _weaponUpgrade.weapon;
		weaponName.SetText(_weaponUpgrade.itemName);
		weaponImage.sprite = _weaponUpgrade.weapon.Sprite;
		forgeManager = _forgeManager;
		WeaponDamage = weapon.Power;
		WeaponRange = weapon.range;
		WeaponCritic = weapon.criticalPercentage;
		WeaponDefense = weapon.Defense;

		for (int i = 0; i < _weaponUpgrade.weapon.Abilities.Length; i++)
		{
			var description =_weaponUpgrade.weapon.Abilities[i].description;
			var name = _weaponUpgrade.weapon.Abilities[i].abilityName;
			abilitiesDescription.Add(description);
			abilitiesName.Add(name);
		  
			
		}
	
			
		UpdateChest();

	}
	
	public void UpdateChest()
	{
			
		if(CheckRevealChest())
		{
			chestSprite.gameObject.SetActive(true);
		}
		else{chestSprite.gameObject.SetActive(false);}
	}
	
	bool CheckRevealChest()
	{
		var equipmentInventory = GameManager.instance.equipmentInventory.container;
		var unitProfileList = GameManager.instance.unitProfilesList;
		
		for (int i = 0; i < equipmentInventory.Count; i++)
		{
			if(equipmentInventory[i].weapon == weapon)
			{
				return true;
			}
		}
		
		for (int a = 0; a < unitProfileList.Length; a++)
		{
			if(unitProfileList[a].unitWeapon == weapon)
			{
				return true;
			}
		}
		
		return false;
	}


   

}
