using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;


public class DisplayEquipmentBunkhouse : MonoBehaviour
{
	public GameObject slotPrefab;
	public EquipmentInventory inventory;
	public BunkhouseManager bunkHouseManager;

	public int unitProfileID = 0;
	public List<GameObject> slotPrefablist = new List<GameObject>();
	public Dictionary<WeaponSlot, GameObject> equipmentDisplayed = new Dictionary<WeaponSlot, GameObject>();
	// void Start()
	// {
	// 	//BunkhouseUnitManager.changeUnitWeaponID += UpdateUnitsProfileID;
	// 	CreateDisplay(bunkHouseManager.currentUnitId);

	// }

	// public void UpdateWeaponImage(int i)
	// {
	// 	// bunkHouseManager.UpdateWeaponIMG(i);
	// 	// bunkHouseManager.FillUnitVariables(i);
	// 	// bunkHouseManager.UpdateDefaultWeaponPanel();

	
	// }
	
	public void UpdateWeaponInfo(int hunterId)
	{
		bunkHouseManager.UpdateNewWeaponEquippedInfo(hunterId);
		CreateDisplay(hunterId);
		bunkHouseManager.hunterSlotList[hunterId].UpdateAbilitiesTooltip();
		
	}

	// public void SetUnitProfileID(int id)
	// {
	// 	Debug.Log("SetUnitProfile ID es " + id);
	// 	unitProfileID = id;
	// 	UpdateUnitsProfileID();
	// }

	private void Update()
	{

		UpdateDisplay();
	}

	public void CreateDisplay(int currentId)
	{
	
		

		for (int i = 0; i < inventory.container.Count; i++)
		{
			if(equipmentDisplayed.ContainsKey(inventory.container[i])) continue;
			
			var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
			slotPrefablist.Add(obj);
			//obj.transform.GetChild(1).GetComponentInChildren<Image>().sprite = inventory.container[i].weapon.Sprite;

			equipmentDisplayed.Add(inventory.container[i], obj);
			if (obj.TryGetComponent(out EquipmentBunkhouseButton button))
			{
				button.FillVariables(inventory, i, this);
			}
		}

		foreach (var item in slotPrefablist)
		{

			if (item.TryGetComponent(out EquipmentBunkhouseButton button))
			{
				Debug.Log("unit profile id es " + currentId);
				button.SetUnitProfileID(currentId);
			}
		}


	}

	 void UpdateDisplay()
	{
		
		for (int i = 0; i < inventory.container.Count; i++)
		{

			if (!equipmentDisplayed.ContainsKey(inventory.container[i]))
			{
				var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
				slotPrefablist.Add(obj);
				obj.transform.GetChild(1).GetComponentInChildren<Image>().sprite = inventory.container[i].weapon.Sprite;
				equipmentDisplayed.Add(inventory.container[i], obj);
			}

			if (slotPrefablist[i].TryGetComponent(out EquipmentBunkhouseButton button))
			{
			   
				button.FillVariables(inventory, i, this);
			}

		}
	}

	void UpdateUnitsProfileID()
	{

		foreach (var item in slotPrefablist)
		{
		   
			if (item.TryGetComponent(out EquipmentBunkhouseButton button))
			{
				Debug.Log("unit profile id es " + unitProfileID);
				button.SetUnitProfileID(unitProfileID);
			}

			//if (item.GetComponent<EquipmentBunkhouseButton>() != null)
			//{
			//    Debug.Log("AAAAAA");
			//}
			//else
			//{
			//    Debug.Log("NO ENTRA");
			//}
		}

	}

	

}
