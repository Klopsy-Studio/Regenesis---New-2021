using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandedUnitStatus : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Text unitName;
    [SerializeField] Image unitPortrait;

    [SerializeField] Slider vitValue;
    [SerializeField] Text movValue;
    [SerializeField] Text defValue;
    [SerializeField] Text edValue;
    [SerializeField] Text powValue;
    [SerializeField] Text crtValue;
    [SerializeField] Text eleValue;

    [SerializeField] Image equipmentIcon;
    [SerializeField] Text equipmentName;


    PlayerUnit currentUnit;

    public void AssignValues(PlayerUnit unit)
    {
        if(currentUnit != unit)
        {
            unitName.text = unit.unitName;

            unitPortrait.sprite = unit.fullUnitPortrait;

            vitValue.maxValue = unit.maxHealth;
            vitValue.value = unit.health;

            movValue.text = unit.weapon.range.ToString();
            defValue.text = unit.weapon.Defense.ToString();
            edValue.text = unit.weapon.WeaponAttackElement.ToString();
            powValue.text = unit.weapon.Power.ToString();
            crtValue.text = unit.weapon.CriticalPercentage.ToString();
            eleValue.text = unit.weapon.ElementPower.ToString();

            equipmentIcon.sprite = unit.weapon.weaponIcon;
            equipmentName.text = unit.weapon.name;

        }
    }
}
