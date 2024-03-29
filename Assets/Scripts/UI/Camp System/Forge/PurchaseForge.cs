using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PurchaseForge : MonoBehaviour, IPointerClickHandler
{
	//[SerializeField] OldForgeManager forgeManager;
	[SerializeField] ForgeManager newForgeManager;
	[HideInInspector] public Weapons weaponToPurchase;
	[SerializeField] Animator animatorPurchase;
	 [SerializeField] Animator animatorNotPurchase;
	public void OnPointerClick(PointerEventData eventData)
	{
		if(newForgeManager.currentWeaponInfoTemplate == null)
		{
			AudioManager.instance.Play("NoPurchase");
			Debug.Log("CurrentWeaponInfoTemplate es null");
			return;
		}
		if(newForgeManager.currentWeaponInfoTemplate.WeaponUpgrade.weapon == null)
		{
			AudioManager.instance.Play("NoPurchase");

			Debug.Log("El weapon que se quiere comprar es null");
			return;
		}

		//Check if it meets the necessary materials and weapon
		if (!newForgeManager.CanPurchaseWeapon())
		{
			AudioManager.instance.Play("NoPurchase");
			animatorNotPurchase.SetTrigger("purchased");
			return;
		}

		//Add weapon to inventory
		var newEquipment = newForgeManager.currentWeaponInfoTemplate.WeaponUpgrade.weapon;
		GameManager.instance.equipmentInventory.AddItem(newEquipment);

		//Reduce Material and/or weapon required
		newForgeManager.ReduceMaterialAndWeapon();

		//UpdateUI
		newForgeManager.UpdateMaterialRequiredPanel(newForgeManager.currentWeaponInfoTemplate.WeaponUpgrade);
		animatorPurchase.SetTrigger("purchased");
		AudioManager.instance.Play("ComprarForja");
		
		newForgeManager.UpdateChest();

	}




}
