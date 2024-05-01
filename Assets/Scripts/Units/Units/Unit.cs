using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//Right now, this script track if the unit is on the board and what direction is facing
//In the future, we should add stats of the unit
public class Unit : TimelineElements
{
    [Space]
    public Directions dir;
    public Tile tile { get; protected set; }
    public float unitSpeed;

    public TimelineElements element;

    public bool stunned;
    public float timeStunned;
    protected float originalTimeStunned;
    public TimelineVelocity previousVelocity;
    [HideInInspector] public BattleController controller;


 
    public override int ActionsPerTurn
    {
        get { return actionsPerTurn; }
        set { actionsPerTurn = value; }
    }


    //When unit uses its action, the turn goes to the next unit
    public bool turnEnded;
    public bool actionDone;
    public Point currentPoint;


    //Variables que se comparten entre unidades del jugador y del enemigo
    public int maxHealth;
    public int health;


    public AnimationClip hurtAnimation;

    public string unitName;

    public GameObject particleHitPrefab;

    public bool isInAction = false;

    [HideInInspector] public bool isDead;

    [Header("Particles")]
    [SerializeField] GameObject healEffect;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Animator criticalMark;
    [SerializeField] Animator battlecryMark;
    public Animator stunEffect;
    [SerializeField] public float stunThreshold;
    [SerializeField] float stunLimit;
    [SerializeField] Animator movementEffect;
    [SerializeField] Animator slowEffect;
    [SerializeField] Animator hasteEffect;

    public SpriteRenderer unitSprite;
    [SerializeField] float fadeDuration;
    bool setFade;
    bool fadeTransition;
    float fadeTime;
    
    [Header("Testing")]
    [SerializeField] bool thisIsMyFuckingTurn;


    [Header("Unit Stats")]
    public int power;
    [HideInInspector] public int originalPower;
    public int criticalPercentage;
    [HideInInspector] public int originalCriticalPercentage;
    public WeaponElement attackElement;
    public WeaponElement defenseElement;
    public int elementPower;
    [HideInInspector] public int originalElementPower;
    public int defense;
    [HideInInspector] public int originalDefense;

    public float criticalDamage = 1.5f;
    [HideInInspector] public float originalCriticalDamage;

    public float damageModifier = 1f;

    [Header("Modifiers")]

    public List<Modifier> buffModifiers;
    public List<Modifier> debuffModifiers;


    [Header("Indicators")]
    [SerializeField] GameObject buffIndicator;
    [SerializeField] GameObject debuffIndicator;
    public GameObject tileIndicator;
    public UnitUI popUps;
    public Animator droneVFX;

    [Header("Effects")]
    [HideInInspector] public ActionEffectParameters currentParameters;

   
    protected virtual void Start()
    {
        buffModifiers = new List<Modifier>();
        debuffModifiers = new List<Modifier>();
        Match();
        SetInitVelocity();
        originalTimeStunned = timeStunned;
        popUps = GetComponent<UnitUI>();
    }

    public void EnableSlow()
    {
        slowEffect.SetTrigger("slow");
    }
    public void EnableHaste()
    {
        hasteEffect.SetTrigger("haste");
    }
    public void AddBuff(Modifier m)
    {
        buffModifiers.Add(m);
        popUps.CreatePopUpBuffIndicator(m);
        buffIndicator.SetActive(true);
        AudioManager.instance.Play("AddBuff");
    }

    public void RemoveBuff(Modifier m)
    {
        popUps.CreateRemoveBuffIndicator(m);
        if (buffModifiers.Contains(m))
        {
            buffModifiers.Remove(m);
        }

        if(buffModifiers.Count<= 0)
        {
            buffIndicator.SetActive(false);
        }
    }
    public void AddDebuff(Modifier m)
    {
        debuffModifiers.Add(m);
        debuffIndicator.SetActive(true);
        popUps.CreatePopUpDebuffIndicator(m);
        AudioManager.instance.Play("AddDebuff");

    }


    public void RemoveDebuff(Modifier m)
    {
        popUps.CreateRemoveDebuffIndicator(m);

        if (debuffModifiers.Contains(m))
        {
            debuffModifiers.Remove(m);
        }

        if (debuffModifiers.Count <= 0)
        {
            debuffIndicator.SetActive(false);
        }
    }
    public void EnableCriticalMark()
    {
        if (criticalMark != null)
        {
            criticalMark.SetTrigger("mark");
        }
        else
        {
            Debug.Log("Critical Mark Missing");
        }
    }

    public virtual void MovementEffect()
    {
        movementEffect.SetTrigger("move");
        AudioManager.instance.Play("HunterMovement");

    }
    private void Update()
    {
        if (thisIsMyFuckingTurn)
        {
            fTimelineVelocity = 100000f;
        }
    }

    public void LateUpdate()
    {
        if (setFade)
        {
            if (fadeTransition)
            {
                fadeTime += Time.deltaTime;

                unitSprite.color = unitSprite.color = new Color(unitSprite.color.r, unitSprite.color.g, unitSprite.color.b, Mathf.Lerp(unitSprite.color.a, 0.5f, fadeTime/0.2f));

                if (fadeTime >= 0.2f)
                {
                    unitSprite.color = unitSprite.color = new Color(unitSprite.color.r, unitSprite.color.g, unitSprite.color.b, 0.5f);
                    fadeTransition = false;
                    fadeTime = 0;
                }
            }
            else
            {
                unitSprite.color = new Color(unitSprite.color.r, unitSprite.color.g, unitSprite.color.b, 0.5f);
            }
        }

        else
        {
            if (fadeTransition)
            {
                fadeTime += Time.deltaTime;

                unitSprite.color = new Color(unitSprite.color.r, unitSprite.color.g, unitSprite.color.b, Mathf.Lerp(0.5f, 1, fadeTime / 0.2f));

                if (fadeTime >= 0.2f)
                {
                    unitSprite.color = unitSprite.color = new Color(unitSprite.color.r, unitSprite.color.g, unitSprite.color.b, 1);
                    fadeTransition = false;
                    fadeTime = 0;
                }
            }
        }
    }
    
    public void SetUnitFade(bool value)
    {
        setFade = value;
        fadeTransition = true;
        fadeTime = 0;
    }
    public void Place(Tile target)
    {
        // Make sure old tile location is not still pointing to this unit
        if (tile != null && tile.content == gameObject)
            tile.content = null;
        // Link unit and tile references
        tile = target;
        currentPoint = tile.pos;
        if (target != null)
            target.content = gameObject;
    }

    public void SetOriginalValues()
    {
        originalPower = power;
        originalCriticalPercentage = criticalPercentage;
        originalElementPower = elementPower;
        originalDefense = defense;
        originalCriticalDamage = criticalDamage;
    }

    public void ResetValues()
    {
        power = originalPower;
        criticalPercentage = originalCriticalPercentage;
        elementPower = originalElementPower;
        defense = originalDefense;
    }
    public void Match()
    {
        if(tile != null)
        {
            transform.localPosition = tile.center;
            currentPoint = tile.pos;
        }     
    }

    public void ApplyStunValue(float value)
    {
        stunThreshold += value;

        if (stunThreshold >= stunLimit)
        {
            stunThreshold = 0;
            FallBackOnTimeline();
        }
    }

    public void PlayAudio(string audioName)
    {
        AudioManager.instance.Play(audioName);
    }
    public void FallBackOnTimeline()
    {
        timelineFill -= 50;

        if (timelineFill <= 0)
        {
            timelineFill = 0;
        }
    }
    public virtual bool ReceiveDamage(int damage, bool isCritical)
    {

        health -= (int)damage;

        if (health <= 0)
        {
            health = 0;
            return true;
        }
        else
        {
            return false;
        }

    }

    public virtual bool ReceiveDamageStun(float damage, float StunDMG)
    {
        health -= (int)damage;

        if (health <= 0)
        {
            health = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void Heal(float heal)
    {
        AudioManager.instance.Play("HunterHeal");
        HealEffect();
        health += (int)heal;

        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void SetInitVelocity()
    {
        if(iconTimeline != null)
        {
            iconTimeline.SetTimelineIconTextVelocity();
        }
        switch (timelineVelocity)
        {
            case TimelineVelocity.VerySlow:
                fTimelineVelocity = 9;
                break;
            case TimelineVelocity.Slow:
                fTimelineVelocity = 12;
                break;
            case TimelineVelocity.Normal:
                fTimelineVelocity = 15;
                break;
            case TimelineVelocity.Quick:
                fTimelineVelocity = 17;
                break;
            case TimelineVelocity.VeryQuick:
                fTimelineVelocity = 19;
                break;
            case TimelineVelocity.TurboFast:
                fTimelineVelocity = 20;
                break;
            default:
                break;
        }
    }

    public virtual void NearDeath()
    {

    }
    public virtual void Die()
    {
        controller.unitsInGame.Remove(this);
        buffIndicator.SetActive(false);
        debuffIndicator.SetActive(false);
    }

    public virtual void Stun()
    {
        if (!stunned)
        {
            //fTimelineVelocity = 0;
            timelineVelocity = TimelineVelocity.Stun;
            AddDebuff(new Modifier { modifierType = TypeOfModifier.Stun });
            previousVelocity = timelineVelocity;
            stunned = true;
            stunEffect.SetTrigger("stun");
            iconTimeline.EnableStun();
            SetCurrentVelocity();
        }
    }
    public void SetCurrentVelocity()
    {
        timelineVelocity += (int)actionsPerTurn;
        
        switch (timelineVelocity)
        {
            case TimelineVelocity.VerySlow:
                fTimelineVelocity = 9;
                break;
            case TimelineVelocity.Slow:
                fTimelineVelocity = 12;
                break;
            case TimelineVelocity.Normal:
                fTimelineVelocity = 15;
                break;
            case TimelineVelocity.Quick:
                fTimelineVelocity = 17;
                break;
            case TimelineVelocity.VeryQuick:
                fTimelineVelocity = 19;
                break;
            case TimelineVelocity.TurboFast:
                fTimelineVelocity = 20;
                break;
            //case TimelineVelocity.Stun:
            //    fTimelineVelocity = 0;
            //break;
            default:
                break;
        }

        if (iconTimeline != null)
        {
            iconTimeline.SetTimelineIconTextVelocity();
        }
    }

    public void DecreaseTimelineVelocity(int decrease)
    {
        timelineVelocity -= decrease;

        switch (timelineVelocity)
        {
            case TimelineVelocity.VerySlow:
                fTimelineVelocity = 9;
                break;
            case TimelineVelocity.Slow:
                fTimelineVelocity = 12f;
                break;
            case TimelineVelocity.Normal:
                fTimelineVelocity = 15;
                break;
            case TimelineVelocity.Quick:
                fTimelineVelocity = 17f;
                break;
            case TimelineVelocity.VeryQuick:
                fTimelineVelocity = 19;
                break;
            case TimelineVelocity.TurboFast:
                fTimelineVelocity = 20f;
                break;
            default:
                break;
        }

        EnableSlow();

        if (iconTimeline != null)
        {
            iconTimeline.SetTimelineIconTextVelocity();
        }
    }

    public void IncreaseTimelineVelocity(int increase)
    {
        timelineVelocity += increase;

        switch (timelineVelocity)
        {
            case TimelineVelocity.VerySlow:
                fTimelineVelocity = 9;
                break;
            case TimelineVelocity.Slow:
                fTimelineVelocity = 12f;
                break;
            case TimelineVelocity.Normal:
                fTimelineVelocity = 15;
                break;
            case TimelineVelocity.Quick:
                fTimelineVelocity = 17f;
                break;
            case TimelineVelocity.VeryQuick:
                fTimelineVelocity = 19;
                break;
            case TimelineVelocity.TurboFast:
                fTimelineVelocity = 20f;
                break;
            default:
                break;
        }

        EnableHaste();

        if (iconTimeline != null)
        {
            iconTimeline.SetTimelineIconTextVelocity();
        }
    }
    public override bool UpdateTimeLine()
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
            Debug.Log("stunned");
            timeStunned -= Time.deltaTime;

            if (timeStunned <= 0)
            {
                timelineVelocity = previousVelocity;
                
                SetCurrentVelocity();
                stunned = false;
                timeStunned = originalTimeStunned;

                Modifier stun = new Modifier();

                foreach(Modifier m in debuffModifiers)
                {
                    if(m.modifierType == TypeOfModifier.Stun)
                    {
                        RemoveDebuff(m);
                        break;
                    }
                }

            }

            return false;
        }

    }

    public void ShakeWithCurrent()
    {
        ActionEffect.instance.Shake(currentParameters);
    }
    public void SetVelocityWhenTurnIsFinished()
    {
        int speed = (int)actionsPerTurn;
        foreach (Modifier m in buffModifiers)
        {
            if(m.modifierType == TypeOfModifier.TimelineSpeed)
            {
                speed += 1;
            }

            if(speed >= 5)
            {
                speed = 5;
            }

        }


        timelineVelocity += speed;

        switch (timelineVelocity)
        {
            case TimelineVelocity.VerySlow:
                fTimelineVelocity = 9;
                break;
            case TimelineVelocity.Slow:
                fTimelineVelocity = 12;
                break;
            case TimelineVelocity.Normal:
                fTimelineVelocity = 15;
                break;
            case TimelineVelocity.Quick:
                fTimelineVelocity = 17;
                break;
            case TimelineVelocity.VeryQuick:
                fTimelineVelocity = 19;
                break;
            case TimelineVelocity.TurboFast:
                fTimelineVelocity = 20;
                break;
            //case TimelineVelocity.Stun:
            //    fTimelineVelocity = 0;
            //    break;
            default:
                break;
        }

        if (iconTimeline != null)
        {
            iconTimeline.SetTimelineIconTextVelocity();
        }
    }

    public void UpdateCurrentVelocity()
    {
        switch (timelineVelocity)
        {
            case TimelineVelocity.VerySlow:
                fTimelineVelocity = 9;
                break;
            case TimelineVelocity.Slow:
                fTimelineVelocity = 12;
                break;
            case TimelineVelocity.Normal:
                fTimelineVelocity = 15;
                break;
            case TimelineVelocity.Quick:
                fTimelineVelocity = 17;
                break;
            case TimelineVelocity.VeryQuick:
                fTimelineVelocity = 19;
                break;
            case TimelineVelocity.TurboFast:
                fTimelineVelocity = 20;
                break;
            default:
                break;
        }

        iconTimeline.SetTimelineIconTextVelocity();

    }
    //public void DebugThings()
    //{
    //    Debug.Log( this.unitName + "current velocity: " + timelineVelocity);
    //}


    public virtual void Damage()
    {

    }

    public virtual void Default()
    {

    }

    public virtual void Attack()
    {

    }


    public void DamageEffect()
    {
        Debug.Log("Effect");
        GameObject temp = Instantiate(hitEffect, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), hitEffect.transform.rotation);
        Destroy(temp, 0.8f);
    }

    public void HealEffect()
    {
        Debug.Log("Heal");
        GameObject temp = Instantiate(healEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), healEffect.transform.rotation);
        Destroy(temp, 0.8f);
    }


    public void EnableBattlecry()
    {
        battlecryMark.SetTrigger("battlecry");
    }

    public void PlayActionEffect()
    {
        ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);
    }

    public void PlayActionEffectWithParammeters(float size, float duration)
    {
        ActionEffect.instance.Play(size, duration, 0.01f, 0.05f);

    }

}
