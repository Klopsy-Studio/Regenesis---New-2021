using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EquipmentSlotButton : MonoBehaviour, IPointerClickHandler
{
    public Image equipmentImage;
    EquipmentInventoryManager equipmentInventoryManager;

    public int WeaponDamage { get; private set; }
    public int WeaponRange { get; private set; }
    public int WeaponCritic { get; private set; }
    public int WeaponDefense { get; private set; }

    public string weaponName;

    public List<string> abilitiesDescription;
    public List<string> abilitiesName;
    public void SetSlotButton(WeaponSlot _weaponSlot)
    {
        equipmentImage.sprite = _weaponSlot.weapon.Sprite;
    }

    public void SetWeaponSlotInfo(EquipmentInventoryManager _equipmentInventoryManager, Weapons _weapon)
    {
        equipmentInventoryManager = _equipmentInventoryManager;

        weaponName = _weapon.EquipmentName;
        WeaponDamage = _weapon.Power;
        WeaponRange = _weapon.range;
        WeaponCritic = _weapon.criticalPercentage;
        WeaponDefense = _weapon.Defense;

        for(int i = 0; i < _weapon.Abilities.Length; i++)
        {
            var description = _weapon.Abilities[i].description;
            var name = _weapon.Abilities[i].abilityName;
            abilitiesDescription.Add(description);
            abilitiesName.Add(name);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("FUNCIONA");
        equipmentInventoryManager.UpdateEquipmentPanelInfo(this);
        for (int i = 0; i < abilitiesDescription.Count; i++)
        {
            equipmentInventoryManager.abilityTooltipList[i].SetAbilityTooltip(abilitiesName[i], abilitiesDescription[i]);
        }
    }
}
