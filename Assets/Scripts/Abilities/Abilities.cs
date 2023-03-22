using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum TypeOfAbilityRange
{
    Cone,
    Constant,
    Infinite,
    LineAbility,
    SelfAbility,
    SquareAbility,
    Side,
    AlternateSide,
    Cross,
    Normal,
    Item,
};

public enum EffectType
{
    Damage,
    Heal,
    Buff,
    Debuff
};

public enum AbilityTargetType
{
    BigMonster, Enemies, Allies, Obstacles, Self, Tile
};
[CreateAssetMenu(menuName = "Ability/New Ability")]
public class Abilities : ScriptableObject
{
    public KitType abilityEquipmentType;
    [Range(1,5)]
    public int actionCost;
    [Range(0, 6)]
    public int ammoCost;

    public List<RangeData> abilityRange;
    public List<RangeData> tileTargetAbilityRange;
    public RangeData rangeData;
    [Header("Effect parameters")]
    [SerializeField] public float cameraSize = 3f;
    [SerializeField] public float effectDuration = 0.5f;
    [SerializeField] public float shakeIntensity = 0.01f;
    [SerializeField] public float shakeDuration = 0.05f;

   
    [Header("Ability Variables")]
 
    public string abilityName;
    public EffectType abilityEffect;
  

    [Header("Damage")]
    //Variables relacionado con daño
    int finalDamage;
    
    [Range(0f, 1f)]
    [SerializeField] public float abilityModifier;
    float originalAbilityModifier;
    [Header("Heal")]
    //Si la habilidad es de curación, se utilizan estas variables
    public float initialHeal;
    float finalHeal;

    [Header("Buff")]
    //Si la habilidad es un bufo, se usará esto
    public float initialBuff;

    [Header("Debuff")]
    //Si la habilidad es de debuffo, se usará esto
    public float initialDebuff;

    
    [Space]
    [Header("Others")]
 
    public int stunDamage;
    public string animationName;
    [HideInInspector] public Unit lastTarget;
    public Weapons weapon;

    [TextArea(15, 20)]
    public string description;
    public int abilityTextFontSize;
    [Header("AbilityEffects")]
    public List<Effect> inAbilityEffects;
    public List<Effect> postAbilityEffect;

    public AbilitySequence sequence;

    [Header("Sound Parameters")]
    public string soundString;

    public List<AbilityTargetType> elementsToTarget;

    [HideInInspector] public bool isCritical;

    public bool CanDoAbility(int actionPoints)
    {
        if(actionPoints < actionCost)
        {
            return false;
        }
        else
        {
            return true;
        }
    }



    public bool CanDoAbility(int actionPoints, PlayerUnit user)
    {
        if(ammoCost == 6)
        {
            if (actionPoints < actionCost || user.gunbladeAmmoAmount <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (actionPoints < actionCost || user.gunbladeAmmoAmount < ammoCost)
            {
                return false;
            }
            else
            {
                return true;
            }
        } 
    }
    //public void UseAbilityAgainstPlayerUnit(Unit target)
    //{
    //    CalculateDmg();
    //    target.ReceiveDamage(finalDamage);
    //}
    
    public int CalculateDmg(Unit user, Unit target)
    {
        originalAbilityModifier = abilityModifier;
        float criticalDmg = 1f;
        bool criticalModifier = false;
        if(target.debuffModifiers != null && target.debuffModifiers.Count > 0)
        {
            List<Modifier> trashModifiers = new List<Modifier>();

            foreach(Modifier d in target.debuffModifiers)
            {
                if(d.modifierType == TypeOfModifier.HunterMark && !criticalModifier)
                {
                    criticalDmg = 1.5f;
                    criticalModifier = true;

                    if (d.SpendModifier())
                    {
                        trashModifiers.Add(d);
                    }
                }
            }

            foreach(Modifier d in trashModifiers)
            {
                target.RemoveDebuff(d);
            }


        }
        else
        {
            if (Random.value * 100 <= user.criticalPercentage)
            {
                criticalDmg = 1.5f;
            }
        }

        float elementEffectivenessNumber = ElementsEffectiveness.GetEffectiveness(user.attackElement, target.defenseElement);

        if(target.buffModifiers!= null)
        {
            if(target.buffModifiers.Count > 0)
            {
                List<Modifier> trash = new List<Modifier>();

                foreach(Modifier d in target.buffModifiers)
                {
                    if(d.modifierType == TypeOfModifier.Defense)
                    {
                        abilityModifier -= d.damageReduction;

                        if (d.SpendModifier())
                        {
                            trash.Add(d);
                        }

                        break;
                    }  
                }

                if(trash.Count > 0)
                {
                    foreach(Modifier d in trash)
                    {
                        target.RemoveBuff(d);
                    }
                }
            }
        }

        if(user.buffModifiers.Count > 0)
        {
            List<Modifier> trash = new List<Modifier>();
            foreach(Modifier m in user.buffModifiers)
            {
                if(m.modifierType == TypeOfModifier.Damage)
                {
                    finalDamage = (int)(((((user.power+(user.power*0.25) * criticalDmg) + (user.power * user.elementPower) * elementEffectivenessNumber) * abilityModifier) - target.defense));

                    if (m.SpendModifier())
                    {
                        trash.Add(m);
                    }
                    break;
                }
            }

            if(trash.Count > 0)
            {
                foreach(Modifier m in trash)
                {
                    user.RemoveBuff(m);
                }
            }
        }

        else
        {
            finalDamage = (int)((((user.power * criticalDmg) + (user.power * user.elementPower) * elementEffectivenessNumber) * abilityModifier) - target.defense);
        }

        if (finalDamage <= 0)
        {
            finalDamage = 0;
        }

        abilityModifier = originalAbilityModifier;

        if(criticalDmg > 1)
        {
            isCritical = true;
        }
        else
        {
            isCritical = false;
        }


        if (target.debuffModifiers != null && target.debuffModifiers.Count > 0)
        {
            List<Modifier> trashModifiers = new List<Modifier>();

            foreach (Modifier d in target.debuffModifiers)
            {
                if (d.modifierType == TypeOfModifier.Damage)
                {
                    user.power = user.originalPower;

                    if (d.SpendModifier())
                    {
                        trashModifiers.Add(d);
                    }
                }
            }

            foreach (Modifier d in trashModifiers)
            {
                target.RemoveDebuff(d);
            }

        }
        target.ResetValues();

        return finalDamage;
    }

    public int CalculateDamageWithCrit(Unit user, Unit target)
    {
        originalAbilityModifier = abilityModifier;
        float criticalDmg = 1f;
        bool criticalModifier = false;
        if (target.debuffModifiers != null && target.debuffModifiers.Count > 0)
        {
            List<Modifier> trashModifiers = new List<Modifier>();

            foreach (Modifier d in target.debuffModifiers)
            {
                if (d.modifierType == TypeOfModifier.HunterMark && !criticalModifier)
                {
                    criticalDmg = 1.5f;
                    criticalModifier = true;

                    if (d.SpendModifier())
                    {
                        trashModifiers.Add(d);
                    }
                }
            }

            foreach (Modifier d in trashModifiers)
            {
                target.RemoveDebuff(d);
            }


        }

        criticalDmg = 1.5f;
        float elementEffectivenessNumber = ElementsEffectiveness.GetEffectiveness(user.attackElement, target.defenseElement);

        if (target.buffModifiers != null)
        {
            if (target.buffModifiers.Count > 0)
            {
                List<Modifier> trash = new List<Modifier>();

                foreach (Modifier d in target.buffModifiers)
                {
                    if (d.modifierType == TypeOfModifier.Defense)
                    {
                        abilityModifier -= d.damageReduction;

                        if (d.SpendModifier())
                        {
                            trash.Add(d);
                        }

                        break;
                    }
                }

                if (trash.Count > 0)
                {
                    foreach (Modifier d in trash)
                    {
                        target.RemoveBuff(d);
                    }
                }
            }
        }

        if (user.buffModifiers.Count > 0)
        {
            List<Modifier> trash = new List<Modifier>();
            foreach (Modifier m in user.buffModifiers)
            {
                if (m.modifierType == TypeOfModifier.Damage)
                {
                    finalDamage = (int)(((((user.power + (user.power * 0.25) * criticalDmg) + (user.power * user.elementPower) * elementEffectivenessNumber) * abilityModifier) - target.defense));

                    if (m.SpendModifier())
                    {
                        trash.Add(m);
                    }
                    break;
                }
            }

            if (trash.Count > 0)
            {
                foreach (Modifier m in trash)
                {
                    user.RemoveBuff(m);
                }
            }
        }

        else
        {
            finalDamage = (int)((((user.power * criticalDmg) + (user.power * user.elementPower) * elementEffectivenessNumber) * abilityModifier) - target.defense);

        }

        if (finalDamage <= 0)
        {
            finalDamage = 0;
        }

        abilityModifier = originalAbilityModifier;

        if (criticalDmg > 1)
        {
            isCritical = true;
        }
        else
        {
            isCritical = false;
        }
        target.ResetValues();

        return finalDamage;
    }

    //When playerUnit does dmg to another playerUnit
    void CalculateDmg(PlayerUnit player,PlayerUnit target)
    {
        float criticalDmg = 1f;
        if (Random.value * 100 <= player.criticalPercentage) criticalDmg = 1.5f;

        float elementDmg = ElementsEffectiveness.GetEffectiveness(player.attackElement, target.defenseElement);
        finalDamage = (int)(((player.power * criticalDmg) + (player.power * player.elementPower) * elementDmg) * abilityModifier) - target.defense;
    }
    void CalculateHeal()
    {
        //Fill with calculate heal code
        finalHeal = initialHeal;
    }

}
