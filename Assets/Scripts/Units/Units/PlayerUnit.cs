using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weasel.Utils;

public class PlayerUnit : Unit
{
    public UnitProfile profile;
    [HideInInspector] public Sprite unitPortrait;
    [HideInInspector] public Sprite fullUnitPortrait;
    public bool didNotMove;
    public Weapons weapon;


    public bool changing;

    [HideInInspector] public UnitStatus status;

    public PlayerUnitUI playerUI;


    [Header("Animations")]
    public UnitAnimations animations;
    [SerializeField] Animator weaponTraitsAnimations;

    [Header("Unit Death")]
    [HideInInspector] public Sprite deathTimelineSprite;
    bool diedOnce = false;
    public PlayerUnitDeath nearDeathElement;
    public PlayerUnitDeath deathElement;

    [HideInInspector] public bool isNearDeath;

    [Header("Weapons")]
    public SpriteRenderer hammerSprite;
    public SpriteRenderer bowSprite;
    public SpriteRenderer bowTensedSprite;
    public SpriteRenderer gunbladeSprite;
    [SerializeField] GameObject hammerParent;
    [SerializeField] GameObject bowParent;
    [SerializeField] GameObject gunbladeParent;

    [Header("Weapon Variables")]
    public int hammerFuryAmount;
    public int hammerFuryMax;
    public int gunbladeAmmoAmount;
    public int gunbladeAmmoMax;
    public PlayerUnit droneUnit;
    [SerializeField] GameObject droneIndicator;
    public int pushAmount;
    public Directions pushDirections;


    [Header("Monster Variables")]
    [HideInInspector] public bool marked;

    [Header("Animation Parameters")]
    [SerializeField] ActionEffectParameters trueShotShakeParameters;
    [SerializeField] float trueShotShakeTime;

    [SerializeField] ActionEffectParameters atomicBarrageShakeParameters;
    [SerializeField] float atomicBarrageShakeTime;


    public Unit currentTarget;
    public List<GameObject> currentTargets;
    public Abilities currentAbility;
    public Modifier currentModifier;
    public List<Tile> abilityTiles;



    protected override void Start()
    {
        base.Start();

        playerUI.unitUI.worldCamera = Camera.main;
        playerUI.unitUI.planeDistance = 0.01f;

        didNotMove = true;
        timelineFill = Random.Range(50, 90);
        //ESTO DEBER�A DE ESTAR EN UNIT

        timelineTypes = TimeLineTypes.PlayerUnit;


        EquipAllItems();
        SetOriginalValues();
        switch (weapon.EquipmentType)
        {
            case KitType.Hammer:
                animations.SetWeapon(0f);
                bowParent.SetActive(false);
                gunbladeParent.SetActive(false);
                break;
            case KitType.Bow:
                animations.SetWeapon(0.5f);

                hammerParent.SetActive(false);
                gunbladeParent.SetActive(false);
                break;
            case KitType.Gunblade:
                animations.SetWeapon(1f);
                hammerParent.SetActive(false);
                bowParent.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void EnableDrone()
    {
        droneIndicator.SetActive(true);
    }

    public void DisableDrone()
    {
        droneIndicator.SetActive(false);
    }
    //ESTA FUNCION HAY QUE REVISARLA
    public void EquipAllItems()
    {
        if (weapon == null) { Debug.Log("No hay weapon"); return; }
        health = 100;
        weapon.EquipItem(this);
    }

    public bool CanMove()
    {
        if (actionsPerTurn >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetDrone(PlayerUnit newUnit, int cost)
    {
        if (newUnit != droneUnit)
        {
            if (droneUnit != null)
            {
                //Update sprite to removeDrone
            }

            droneUnit = newUnit;

            SpendActionPoints(cost);
            playerUI.ShowActionPoints();
            //Update with drone sprite on new unit;
        }

    }

    public void PushTarget()
    {
        if(pushAmount > 0)
        {
            if (currentTarget != null)
            {
                currentTarget.GetComponent<Movement>().PushUnit(pushDirections, pushAmount, controller.board);
            }
        }
    }
    public void AddBattlecryToUnits()
    {
        foreach (GameObject u in currentTargets)
        {
            if (u.GetComponent<Unit>() != null)
            {
                Unit e = u.GetComponent<Unit>();
                e.AddBuff(currentModifier);
                e.EnableBattlecry();
            }
        }
    }
    public bool CanDoAbility()
    {
        if (weapon.EquipmentType == KitType.Gunblade)
        {
            foreach (Abilities a in weapon.Abilities)
            {
                if (a.CanDoAbility(actionsPerTurn, this))
                {
                    return true;
                }
            }
        }
        else
        {
            foreach (Abilities a in weapon.Abilities)
            {
                if (a.CanDoAbility(actionsPerTurn))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void NearDeathSprite()
    {
        animations.SetNearDeath();
    }

    public override void Damage()
    {
        animations.SetDamage();
    }
    public override void Attack()
    {
        animations.SetAnimation("attack");
    }

    public override void Default()
    {
        animations.SetIdle();
    }

    public void Push()
    {
        animations.SetPush();
    }


    public void PlaySmokeVFXAbilityTiles()
    {
        foreach (Tile t in abilityTiles)
        {
            t.SetSmokeBomb();
        }
    }

    public void PlayHealthVFXAbilityTiles()
    {
        foreach (Tile t in abilityTiles)
        {
            t.SetHealthGas();
        }
    }


    public void WeaponOut()
    {
        animations.SetCombatIdle();

        //For later, to take out each weapon depending on type

        //switch (weapon.EquipmentType)
        //{
        //    case EquipmentType.Hammer:
        //        hammerData.savedWeaponSprite.gameObject.SetActive(false);
        //        hammerData.idleCombatSprite.gameObject.SetActive(true);
        //        break;
        //    case EquipmentType.Slingshot:
        //        slingShotData.savedWeaponSprite.gameObject.SetActive(false);
        //        slingShotData.idleCombatSprite.gameObject.SetActive(true);
        //        break;
        //    default:
        //        break;
        //}
    }

    public void AbilityAttack()
    {
        if (currentTarget != null)
        {
            currentTarget.ReceiveDamage(currentAbility.CalculateDmg(this, currentTarget), currentAbility.isCritical);
            Debug.Log("Attacked");
        }
    }
    public void Attack(Unit u)
    {
        if (u != null)
        {
            u.ReceiveDamage(currentAbility.CalculateDmg(this, u), currentAbility.isCritical);
        }
    }
    public void AbilityAttackGroup()
    {
        if (currentTargets != null)
        {
            if (currentTargets.Count > 0)
            {
                foreach (GameObject o in currentTargets)
                {
                    if (o.GetComponent<Unit>())
                    {
                        Attack(o.GetComponent<Unit>());
                    }
                    else if (o.GetComponent<BearObstacleScript>())
                    {
                        o.GetComponent<BearObstacleScript>().GetDestroyed(controller.board);
                    }
                }
            }
        }
    }
    public void AbilityHealGroup()
    {
        if (currentTargets != null)
        {
            if (currentTargets.Count > 0)
            {
                foreach (GameObject o in currentTargets)
                {
                    if (o.GetComponent<Unit>())
                    {
                        o.GetComponent<Unit>().Heal(currentAbility.initialHeal);
                    }
                }
            }
        }
    }

    public void SetMarkOnTarget()
    {
        currentTarget.EnableCriticalMark();
    }
    public void AnimationLog()
    {
        Debug.Log("This animation is played");
    }
    public void CurrentAbilityZoom()
    {
        if (currentAbility != null)
        {
            ActionEffect.instance.Play(currentAbility.shakeParameters);
        }
    }

    public void PlayAbilityShake()
    {
        if (currentAbility != null)
        {
            ActionEffect.instance.Shake(currentAbility.shakeParameters);
        }
    }

    public void TrueShotShake()
    {

        ActivateShake(trueShotShakeParameters, trueShotShakeTime);
    }

    void ActivateShake(ActionEffectParameters parameters, float time)
    {
        ActionEffect.instance.Shake(parameters);
    }
    public void PlayActionEffectAbility()
    {
        if(currentAbility != null)
        {
            ActionEffect.instance.Play(currentAbility.cameraSize, currentAbility.effectDuration, currentAbility.shakeIntensity, currentAbility.shakeDuration);
        }
    }
    public override void NearDeath()
    {
        NearDeathSprite();
        diedOnce = true;
        PlayerUnitDeath element = Instantiate(nearDeathElement);
        element.timelineIcon = deathTimelineSprite;
        //status.unitPortrait.sprite = deathTimelineSprite;
        isNearDeath = true;
        deathElement = element;
        deathElement.unit = this;
        controller.timelineElements.Add(element);
        iconTimeline.gameObject.SetActive(false);
        timelineTypes = TimeLineTypes.Null;
        controller.CheckAllUnits();
    }

    public void Revive()
    {
        deathElement.DisableDeath(controller);
        elementEnabled = true;
        //status.unitPortrait.sprite = timelineIcon;
        Default();
        playerUI.gameObject.SetActive(true);
        timelineFill = 0;
        iconTimeline.gameObject.SetActive(true);
        isNearDeath = false;
        timelineTypes = TimeLineTypes.PlayerUnit;
    }
    public override void Die()
    {
        base.Die();
        controller.playerUnits.Remove(this);
        isNearDeath = true;

        if(weapon.EquipmentType == KitType.Drone)
        {
            if(droneUnit != null)
            {
                droneUnit.DisableDrone();
            }
            droneUnit = null;
        }

        else
        {
            foreach (PlayerUnit u in controller.playerUnits)
            {
                if (u.droneUnit == this)
                {
                    DisableDrone();
                    u.droneUnit = null;
                    //DeactivateSprite
                }
            }
        }
        
        controller.timelineElements.Remove(this);
        elementEnabled = false;
        animations.SetDeath();
        AudioManager.instance.Play("HunterDeath");
        isDead = true;
        controller.CheckAllUnits();
        Destroy(this);

    }

    public override void Stun()
    {
        base.Stun();
        Push();
    }

    public override bool UpdateTimeLine()
    {
        if (!isNearDeath)
        {
            if (!stunned)
            {
                if (timelineFill >= timelineFull)
                {
                    return true;
                }

                timelineFill += fTimelineVelocity * Time.deltaTime;

                return false;
            }

            else
            {
                timeStunned -= Time.deltaTime;

                if (timeStunned <= 0)
                {

                    timelineVelocity = previousVelocity;
                    UpdateCurrentVelocity();
                    stunned = false;
                    timeStunned = originalTimeStunned;

                    foreach (Modifier m in debuffModifiers)
                    {
                        if (m.modifierType == TypeOfModifier.Stun)
                        {
                            RemoveDebuff(m);
                            break;
                        }
                    }
                    //playerUI.DisableStun();
                    iconTimeline.velocityText.gameObject.SetActive(true);
                    iconTimeline.SetTimelineIconTextVelocity();

                    //Disabling Stun icon for now
                    //iconTimeline.DisableStun();
                }

                return false;
            }
        }

        else
        {
            Debug.Log("Near Death");
            return false;
        }


    }

    public void IncreaseBullets(int ammount)
    {
        if (gunbladeAmmoAmount >= gunbladeAmmoMax)
            return;

        gunbladeAmmoAmount += ammount;
        //playerUI.GainBullets(ammount);
        if (gunbladeAmmoAmount > gunbladeAmmoMax)
        {
            gunbladeAmmoAmount = gunbladeAmmoMax;
        }
    }
    public void SpendBullets(int ammount)
    {
        gunbladeAmmoAmount -= ammount;
        //playerUI.SpendBullets(ammount);
        if (gunbladeAmmoAmount <= 0)
        {
            gunbladeAmmoAmount = 0;
        }
    }
    public List<Tile> GetSurroundings(Board board)
    {
        CrossAbilityRange range = this.gameObject.GetComponent<CrossAbilityRange>();

        range.crossLength = 2;
        range.offset = 1;
        range.unit = this;

        return range.GetTilesInRange(board);
    }

    public override bool ReceiveDamage(int damage, bool isCritical)
    {
        health -= (int)damage;

        //status.HealthAnimation(health);
        playerUI.HealthAnimation(health);
        DamageEffect();
        Debug.Log("Damaged");
        if (health <= 0)
        {
            health = 0;
            if (!diedOnce)
            {
                diedOnce = true;
                NearDeath();
                Debug.Log("Near Death");
                //NearDeathSprite();
                health = 0;
                return true;
            }
            else
            {
                if (!isNearDeath)
                {
                    Die();
                    Debug.Log("Dead");
                    NearDeathSprite();
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }


        else
        {
            Damage();
            return false;
        }
    }

    public override void Heal(float heal)
    {
        AudioManager.instance.Play("HunterHeal");

        health += (int)heal;

        playerUI.HealthAnimation(health);

        //status.HealthAnimation(health);
        HealEffect();

        if (isNearDeath)
        {
            Revive();
        }
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }


    public void SpendActionPoints(int actionPoints)
    {
        actionsPerTurn -= actionPoints;
        playerUI.SpendActionPoints(actionPoints);
    }


    public void EnableBowTrait()
    {
        weaponTraitsAnimations.SetBool("idle", false);

        weaponTraitsAnimations.SetTrigger("bow");
    }

    public void ResetWeaponTraits()
    {
        weaponTraitsAnimations.SetBool("idle", true);
    }

    public void EnableHammerTrait()
    {
        weaponTraitsAnimations.SetBool("idle", false);
        weaponTraitsAnimations.SetTrigger("hammer");
    }
}

