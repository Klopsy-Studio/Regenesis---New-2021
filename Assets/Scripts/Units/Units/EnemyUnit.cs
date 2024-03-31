using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    [Header("Enemy Type")]
    public TypeOfEnemy enemyType;
    [Space]
    [Header("Abilities")]
    public Abilities[] abilities;
    public Unit target;
    [SerializeField] WeaponElement monsterAttackElement;
    public WeaponElement MonsterAttackElement { get { return monsterAttackElement; } }

    [SerializeField] WeaponElement monsterDefenseElement;
    public WeaponElement MonsterDefenseElement { get { return monsterDefenseElement; } }

    //Enemy Stats

    public Sprite unitPortrait;
    public MonsterController monsterControl;

    public int lowHealth;
    List<Tile> monsterSpace;

    [Header("UI")]
    [SerializeField] Vector3 originalSpritePosition;
    [SerializeField] Vector3 flippedSpritePosition;

    public Sprite evolvedPortrait;
    public void BeginAnimation()
    {
        monsterControl.animPlaying = true;
    }

    public void EndAnimation()
    {
        monsterControl.animPlaying = false;
    }

    protected override void Start()
    {
        base.Start();
        //timelineFill = Random.Range(0, 3);

        timelineTypes = TimeLineTypes.EnemyUnit;
        health = maxHealth;
        originalSpritePosition = unitSprite.transform.localPosition;
        SetOriginalValues();
    }

    
    public override bool ReceiveDamageStun(float damage, float StunDMG)
    {
        stunThreshold -= StunDMG;
        if (stunThreshold <= 0)
        {
            timelineFill -= 30;
            stunThreshold = 100;
        }
        return base.ReceiveDamageStun(damage, StunDMG);     
    }

    
 
  
    public virtual void UpdateMonsterSpace(Board board)
    {
        if(monsterSpace != null)
        {
            foreach(Tile t in monsterSpace)
            {
                t.occupied = false;
                t.OnUnitLeave();
            }

            monsterSpace.Clear();
        }

        SquareAbilityRange monsterRange = GetComponent<SquareAbilityRange>();
        monsterSpace = monsterRange.GetTilesInRange(board);

        foreach (Tile t in monsterSpace)
        {
            t.occupied = true;
            t.OnUnitArriveMonster(this);
        }
    }

    public List<Tile> GiveMonsterSpace(Board board)
    {

        SquareAbilityRange monsterRange = GetComponent<SquareAbilityRange>();
        monsterRange.squareReach = 1;

        return monsterRange.GetTilesInRange(board);
    }


    public override void Die()
    {
        base.Die();
        monsterControl.monsterAnimations.SetBool("death", true);
        
        isDead = true;
        AudioManager.instance.FadeOut("Music");
        controller.ChangeState<WinState>();
    }
    public void UpdateEnemyUnitSprite()
    {
        //if(monsterControl.target != null)
        //{
        //    if (tile.CheckSpecificDirection(monsterControl.target.tile, Directions.East) || tile.CheckSpecificDirection(monsterControl.target.tile, Directions.South))
        //    {
        //        unitSprite.transform.localPosition = originalSpritePosition;
        //        unitSprite.flipX = false;
        //    }

        //    if (tile.CheckSpecificDirection(monsterControl.target.tile, Directions.West) || tile.CheckSpecificDirection(monsterControl.target.tile, Directions.North))
        //    {
        //        unitSprite.transform.localPosition = flippedSpritePosition;
        //        unitSprite.flipX = true;
        //    }
        //}


        if (unitSprite.flipX)
        {
            unitSprite.transform.localPosition = originalSpritePosition;
            unitSprite.flipX = false;
        }
        else
        {
            unitSprite.transform.localPosition = flippedSpritePosition;
            unitSprite.flipX = true;
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
            timeStunned -= Time.deltaTime;

            if (timeStunned <= 0)
            {
                timelineVelocity = previousVelocity;
                UpdateCurrentVelocity();
                stunned = false;
                timeStunned = originalTimeStunned;
                iconTimeline.DisableStun();
                foreach (Modifier m in debuffModifiers)
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

    public override void Heal(float heal)
    {
        base.Heal(heal);
        if (health >= lowHealth)
        {
            foreach (Modifier m in debuffModifiers)
            {
                if (m.modifierType == TypeOfModifier.NearDeath)
                {
                    RemoveDebuff(m);
                    break;
                }
            }

            monsterControl.monsterAnimations.SetFloat("health", 0);
        }
    }
    public override void Stun()
    {
        if (!stunned)
        {
            //fTimelineVelocity = 0;
            timelineVelocity = TimelineVelocity.Stun;
            iconTimeline.EnableStun();
            AddDebuff(new Modifier { modifierType = TypeOfModifier.Stun });
            previousVelocity = timelineVelocity;
            Debug.Log(gameObject.name + "previousVelocity es " + previousVelocity);
            stunned = true;
            stunEffect.SetTrigger("stun");
            monsterControl.monsterAnimations.SetTrigger("stun");
            SetCurrentVelocity();
        }
    }
    public override bool ReceiveDamage(int damage, bool isCritical)
    {
        DamageEffect();
        if(health-damage <= 0)
        {
            Time.timeScale = 0.3f;
            Invoke("ResetTimeScale", 1f);
        }
        health -= (int)damage;

        List<Modifier> trashModifiers = new List<Modifier>();
        if (debuffModifiers.Count > 0)
        {
            foreach(Modifier m in debuffModifiers)
            {
                if (m.modifierType == TypeOfModifier.Defense)
                {
                    defense = originalDefense;
                    trashModifiers.Add(m);
                }
            }
        }
        

        if(trashModifiers.Count > 0)
        {
            foreach (Modifier m in trashModifiers)
            {
                RemoveDebuff(m);
            }
        }
        //DamageEffect();
        popUps.CreatePopUpText(transform.position + new Vector3(0, 1, 0), (int)damage, isCritical);

        monsterControl.monsterAnimations.SetTrigger("damage");

        if (health <= lowHealth)
        {
            AddDebuff(new Modifier { modifierType = TypeOfModifier.NearDeath });
            monsterControl.monsterAnimations.SetFloat("health", 1);
        }

        

        if (health <= 0)
        {
            health = 0;
            Die();
            return true;
        }

        else
        {
            return false;
        }
    }


    public void ResetTimeScale()
    {
        Time.timeScale = 1f;
    }
   
}

public enum TypeOfEnemy
{
    Big, Small
}
