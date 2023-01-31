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
    Enemies, Allies, Obstacles, Self, Tile
};
[CreateAssetMenu(menuName = "Ability/New Ability")]
public class Abilities : ScriptableObject
{
    public KitType abilityEquipmentType;
    [Range(1,5)]
    public int actionCost;
    [Range(0, 5)]
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
    [SerializeField] EffectType abilityEffect;
  

    [Header("Damage")]
    //Variables relacionado con daño
    float finalDamage;
    
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
        if (actionPoints < actionCost || user.gunbladeAmmoAmount < ammoCost)
        {
            return false;
        }
        else
        {
            return true;
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

        if(target.criticalModifiers != null && target.criticalModifiers.Count > 0)
        {
            List<DamageModifier> trashModifiers = new List<DamageModifier>();

            foreach(DamageModifier d in target.criticalModifiers)
            {
                if (d.SpendModifier())
                {
                    trashModifiers.Add(d);
                    criticalDmg = 1.5f;
                }
            }

            foreach(DamageModifier d in trashModifiers)
            {
                target.criticalModifiers.Remove(d);
            }

            if(target.criticalModifiers.Count == 0)
            {
                target.DisableCriticalMark();
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

        if(target.defenseModifier!= null)
        {
            if(target.defenseModifier.Count > 0)
            {
                List<DamageModifier> trash = new List<DamageModifier>();

                foreach(DamageModifier d in target.defenseModifier)
                {
                    abilityModifier -= d.damageReduction;

                    if (d.SpendModifier())
                    {
                        trash.Add(d);
                    }
                }

                if(trash.Count > 0)
                {
                    foreach(DamageModifier d in trash)
                    {
                        target.defenseModifier.Remove(d);
                    }
                }
            }
        }

        finalDamage = (((user.power * criticalDmg) + (user.power * user.elementPower) * elementEffectivenessNumber) * abilityModifier) - target.defense;

        if(finalDamage <= 0)
        {
            finalDamage = 0;
        }

        abilityModifier = originalAbilityModifier;
        target.ResetValues();

        return (int)finalDamage;
    }


    //When playerUnit does dmg to another playerUnit
    void CalculateDmg(PlayerUnit player,PlayerUnit target)
    {
        float criticalDmg = 1f;

        if(target.criticalModifiers!= null)
        {
            if (Random.value * 100 <= player.criticalPercentage) criticalDmg = 1.5f;
        }

        float elementDmg = ElementsEffectiveness.GetEffectiveness(player.attackElement, target.defenseElement);
        finalDamage = (((player.power * criticalDmg) + (player.power * player.elementPower) * elementDmg) * abilityModifier) - target.defense;
    }
    void CalculateHeal()
    {
        //Fill with calculate heal code
        finalHeal = initialHeal;
    }


  
    public void UseAbility(Unit target, BattleController controller)
    {
        //AQUI YA NO SE HACE EL ACTION COST
        //target.ActionsPerTurn -= ActionCost;
        //controller.currentUnit.actionsPerTurn -= actionCost;

        switch (abilityEffect)
        {
            case EffectType.Damage:
                AudioManager.instance.Play(soundString);
                target.DamageEffect();

                if (target.GetComponent<EnemyUnit>())
                {
                    CalculateDmg(controller.currentUnit,target.GetComponent<EnemyUnit>());
                    if (target.ReceiveDamage(finalDamage))
                    {
                        ActionEffect.instance.Play(cameraSize, effectDuration, shakeIntensity, shakeDuration);
                        target.GetComponent<EnemyUnit>().Die();
                    }
                    else
                    {
                        ActionEffect.instance.Play(cameraSize, effectDuration, shakeIntensity, shakeDuration);
                    }

                    target.GetComponent<UnitUI>().CreatePopUpText(target.transform.position, (int)finalDamage);

                }
                else
                {
                    PlayerUnit u = target.GetComponent<PlayerUnit>();
                    ActionEffect.instance.Play(cameraSize, effectDuration, shakeIntensity, shakeDuration);

                    CalculateDmg(controller.currentUnit,u);
                    u.ReceiveDamage(finalDamage);
                }

                break;
                
            case EffectType.Heal:
                CalculateHeal();
                target.Heal(finalHeal);
                ActionEffect.instance.Play(cameraSize, effectDuration, shakeIntensity, shakeDuration);

                break;
            case EffectType.Buff:
                //Fill with buff code
                break;
            case EffectType.Debuff:
                //Fill with debuff code
                break;
            default:
                break;
        }
    }



}
