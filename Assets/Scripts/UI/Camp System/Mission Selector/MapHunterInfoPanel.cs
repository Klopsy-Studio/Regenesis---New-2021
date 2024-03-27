using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Security;

public class MapHunterInfoPanel : MonoBehaviour
{
	[SerializeField] HunterInfo[] hunterInfo;
	
	// Start is called before the first frame update
	void Start()
	{
		
		FillUnitVariables();
	}

	void FillUnitVariables()
	{
		var unitsWeaponInfo = GameManager.instance.unitProfilesList;
		for (int i = 0; i < unitsWeaponInfo.Length; i++)
		{
			hunterInfo[i].SetHunterInfo(unitsWeaponInfo[i].unitWeapon);
		}
	}
	
	void OnEnable()
	{
		FillUnitVariables();
	}
}

[System.Serializable]
public class HunterInfo
{

	public string unitName;
	public Image weaponIMG;
	public TextMeshProUGUI range;
	public TextMeshProUGUI def;

	public TextMeshProUGUI power;
	public TextMeshProUGUI crit;

	public void SetHunterInfo(Weapons weapon)
	{
		weaponIMG.sprite = weapon.Sprite;
		range.SetText(weapon.range.ToString());
		def.SetText(weapon.Defense.ToString());
	  
		power.SetText(weapon.Power.ToString());
		crit.SetText(weapon.CriticalPercentage.ToString() + "%");
  
	}
}
