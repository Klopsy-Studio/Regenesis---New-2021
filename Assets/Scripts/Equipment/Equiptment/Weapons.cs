using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/New weapon")]
public class Weapons : Equipment
{
    public int criticalPercentage;
    [SerializeField] int originalCriticalPercentage;
    public int CriticalPercentage { get { return criticalPercentage; } }

    [SerializeField] WeaponElement weaponAttackElement;
    [SerializeField] WeaponElement originalWeaponAttackElement;
    public WeaponElement WeaponAttackElement { get { return weaponAttackElement; } }

    [SerializeField] WeaponElement weaponDefenseElement;
    [SerializeField] WeaponElement originalWeaponDefenseElement;
    public WeaponElement WeaponDefenseElement { get { return weaponDefenseElement; } }

    [SerializeField] private int power;
    [SerializeField] int originalPower;
    public int Power { get { return power; } }

    [SerializeField] private int elementPower;
    [SerializeField] int originalElementPower;
    public int ElementPower { get { return elementPower; } }

    [SerializeField] private int defense;
    [SerializeField] int originalDefense;
    public int Defense { get { return defense; } }

    public int range;

    [SerializeField] int originalRange;

    public Abilities[] Abilities;

    public Sprite weaponSprite;
    public Sprite weaponCombat;

    //Extra sprites if necessary
    public Sprite bowTensed;
    
    public Sprite weaponIcon;

    public override void EquipItem(PlayerUnit c)
    {
        c.power = Power;
        c.criticalPercentage = CriticalPercentage;
        c.attackElement = WeaponAttackElement;
        c.defenseElement = WeaponDefenseElement;
        c.elementPower = ElementPower;
        c.defense = Defense;

        foreach(Abilities a in Abilities)
        {
            if(a.sequence != null)
            {
                a.sequence.user = c;
                a.weapon = this;
                a.sequence.ability = a;
            }
        }

        switch (EquipmentType)
        {
            case KitType.Hammer:
                c.hammerSprite.sprite = weaponSprite;
                break;
            case KitType.Bow:
                c.bowSprite.sprite = weaponSprite;
                c.bowTensedSprite.sprite = bowTensed;
                break;
            case KitType.Gunblade:
                c.gunbladeSprite.sprite = weaponSprite;
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        //range = originalRange;
    }


    public void SetDefaultValues()
    {
        power = originalPower;
        criticalPercentage = originalCriticalPercentage;
        weaponAttackElement = originalWeaponAttackElement;
        weaponDefenseElement = originalWeaponDefenseElement;
        elementPower = originalElementPower;
        defense = originalDefense;
    }

}
