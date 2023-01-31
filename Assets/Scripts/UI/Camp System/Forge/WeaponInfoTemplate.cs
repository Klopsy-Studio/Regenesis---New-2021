using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class WeaponInfoTemplate : MonoBehaviour, IPointerClickHandler
{
    public int WeaponDamage { get; private set; }
    public int WeaponRange { get; private set; }
    public int WeaponCritic { get; private set; }
    public int WeaponDefense { get; private set; }
    //public int WeaponElementalDefense { get; private set; }

    ForgeManager forgeManager;
    public TextMeshProUGUI weaponName;
    public WeaponUpgrade WeaponUpgrade { get; private set; }

    public void OnPointerClick(PointerEventData eventData)
    {

        forgeManager.weaponPanelInfo.UpdatePanelInfo(this);
        forgeManager.UpdateMaterialRequiredPanel(WeaponUpgrade);
        forgeManager.SelectCurrentWeaponPanelInfo(this);
        //foreach (var material in WeaponUpgrade.materialsRequired)
        //{
        //    Debug.Log("El material requerido es " + material.monsterMaterial.materialName);
        //}

    }

    public void SetWeaponInfo(WeaponUpgrade _weaponUpgrade, ForgeManager _forgeManager)
    {
        WeaponUpgrade = _weaponUpgrade;
        var weapon = _weaponUpgrade.weapon;
        weaponName.SetText(_weaponUpgrade.itemName);
        forgeManager = _forgeManager;
        WeaponDamage = weapon.Power;
        WeaponRange = weapon.range;
        WeaponCritic = weapon.criticalPercentage;
        WeaponDefense = weapon.Defense;


    }


   

}
