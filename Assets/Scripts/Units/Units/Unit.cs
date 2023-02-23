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
    [SerializeField] Animator stunEffect;
    [SerializeField] public float stunThreshold;
    [SerializeField] float stunLimit;

    public SpriteRenderer unitSprite;

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


    [Header("Modifiers")]

    public List<Modifier> buffModifiers;
    public List<Modifier> debuffModifiers;


    [Header("Indicators")]
    [SerializeField] GameObject buffIndicator;
    [SerializeField] GameObject debuffIndicator;
    protected virtual void Start()
    {
        buffModifiers = new List<Modifier>();
        debuffModifiers = new List<Modifier>();

        Match();
        SetInitVelocity();
        originalTimeStunned = timeStunned;
    }

    public void AddBuff(Modifier m)
    {
        buffModifiers.Add(m);
        buffIndicator.SetActive(true);
    }

    public void RemoveBuff(Modifier m)
    {
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
    }

    public void RemoveDebuff(Modifier m)
    {
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


    private void Update()
    {
        if (thisIsMyFuckingTurn)
        {
            fTimelineVelocity = 100000f;
        }

    }
    public void Place(Tile target)
    {
        // Make sure old tile location is not still pointing to this unit
        if (tile != null && tile.content == gameObject)
            tile.content = null;
        // Link unit and tile references
        tile = target;

        if (target != null)
            target.content = gameObject;
    }

    public void SetOriginalValues()
    {
        originalPower = power;
        originalCriticalPercentage = criticalPercentage;
        originalElementPower = elementPower;
        originalDefense = defense;
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

    public void FallBackOnTimeline()
    {
        timelineFill -= 50;

        if (timelineFill <= 0)
        {
            timelineFill = 0;
        }
    }
    public virtual bool ReceiveDamage(float damage)
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

        health += (int)heal;

        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void SetInitVelocity()
    {
        //SetTimelineVelocityText();
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
        
    }

    public virtual void Stun()
    {
        if (!stunned)
        {
            //fTimelineVelocity = 0;
            timelineVelocity = TimelineVelocity.Stun;
            AddDebuff(new Modifier { modifierType = TypeOfModifier.Stun });
            previousVelocity = timelineVelocity;
            Debug.Log(gameObject.name + "previousVelocity es " + previousVelocity);
            stunned = true;
            stunEffect.SetTrigger("stun");
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
            //Debug.Log(gameObject.name + "timelineFill " + timelineFill);

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
                        stun = m;
                        break;
                    }
                }

                if (debuffModifiers.Contains(stun))
                {
                    RemoveDebuff(stun);
                }
            }

            return false;
        }

    }

    public void SetVelocityWhenTurnIsFinished()
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
            //    break;
            default:
                break;
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
}
