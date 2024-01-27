using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BunkhouseManager : MonoBehaviour
{
	[SerializeField] List<Image> huntersWeaponImg = new List<Image>();
	[SerializeField] SetWeaponInfo weaponInfo;
	Weapons unitWeapon;
	
	
	
	void Start()
	{
		UpdateHuntersWeaponsImg();
	}
	
	void UpdateHuntersWeaponsImg()
	{
		for (int i = 0; i <  GameManager.instance.unitProfilesList.Length; i++)
		{
			unitWeapon = GameManager.instance.unitProfilesList[i].unitWeapon;
			huntersWeaponImg[i].sprite = unitWeapon.Sprite;
			
		}
		weaponInfo.UpdateWeaponStats(GameManager.instance.unitProfilesList[0].unitWeapon);
	}
}

[System.Serializable]
public class SetWeaponInfo
{
	public TextMeshProUGUI range;
	public TextMeshProUGUI def;

	public TextMeshProUGUI power;
	public TextMeshProUGUI crit;
	
	public void UpdateWeaponStats(Weapons weapons)
	{
		range.SetText(weapons.range.ToString());
		def.SetText(weapons.Defense.ToString());
		power.SetText(weapons.Power.ToString());
		crit.SetText(weapons.CriticalPercentage.ToString() + "%");
		
	}
	
	 
}
