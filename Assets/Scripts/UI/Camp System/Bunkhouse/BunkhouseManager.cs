using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BunkhouseManager : MonoBehaviour, IDataPersistence
{
	
	[SerializeField] GameObject tutorialPanel;
	bool isTutorialFinished = false;
	public  SetWeaponInfo weaponInfo;
	Weapons unitWeapon;
	[SerializeField] BunkhouseWeaponSlot hunterSlot;
	public List<BunkhouseWeaponSlot> hunterSlotList = new List<BunkhouseWeaponSlot>();
	[SerializeField] Transform content;
	
	public delegate void WeaponButtonSlotClicked();
	public static event WeaponButtonSlotClicked OnWeaponButtonCliked;
	public AbilityTooltip[] abilityTooltipList;
	
	[SerializeField] DisplayEquipmentBunkhouse displayEquipmentBunkhouse;
	[SerializeField] GameObject selectWeaponPanel;
	
	public int currentUnitId;
	
	void Start()
	{
		tutorialPanel.SetActive(false);
		if (!isTutorialFinished) { tutorialPanel.SetActive(true); }
		FirstUpdateHuntersInfo();
	}
	
		public void FinishTutorial() //UnityButtons
	{
		tutorialPanel.SetActive(false);
		isTutorialFinished = true;
	}
	void FirstUpdateHuntersInfo()
	{
		currentUnitId =0;
		for (int i = 0; i <  GameManager.instance.unitProfilesList.Length; i++)
		{
			unitWeapon = GameManager.instance.unitProfilesList[i].unitWeapon;
			var hunter = Instantiate(hunterSlot,  Vector3.zero, Quaternion.identity, content);
			hunterSlotList.Add(hunter);
			hunter.UpdateWeaponStats(unitWeapon, this, i);
			
		}
		weaponInfo.UpdateWeaponStats(GameManager.instance.unitProfilesList[0].unitWeapon);
		UpdateDisplayEquipmentBunkhousePanel();
	}
	

	void UpdateDisplayEquipmentBunkhousePanel()
	{
		displayEquipmentBunkhouse.CreateDisplay(currentUnitId);
	}
	
	public void WeaponButtonClicked()
	{
		OnWeaponButtonCliked?.Invoke();
		
		
	}
	
	public void UpdateNewSelectedHunterInfo(Weapons weapon, int id)
	{
		weaponInfo.UpdateWeaponStats(weapon);
		currentUnitId = id;
		UpdateDisplayEquipmentBunkhousePanel();
	}
	
	public void UpdateNewWeaponEquippedInfo(int id)
	{
		var weapon = GameManager.instance.unitProfilesList[id].unitWeapon;
		weaponInfo.UpdateWeaponStats(GameManager.instance.unitProfilesList[id].unitWeapon);
		hunterSlotList[id].UpdateWeaponStats(weapon, this, id);
		
	
		
	}
	
	 public void LoadData(GameData data)
	{
		isTutorialFinished = data.isBarrackTutorialFinished;
	}

	public void SaveData(GameData data)
	{
		data.isBarrackTutorialFinished = isTutorialFinished;
	}
	
	

}

[System.Serializable]
public class SetWeaponInfo
{
	
	public TextMeshProUGUI range;
	public TextMeshProUGUI def;

	public TextMeshProUGUI power;
	public TextMeshProUGUI crit;
	
	public void UpdateWeaponStats(Weapons weapon)
	{
	
		range.SetText(weapon.range.ToString());
		def.SetText(weapon.Defense.ToString());
		power.SetText(weapon.Power.ToString());
		crit.SetText(weapon.CriticalPercentage.ToString() + "%");
	
		
	}
	
	 
}
