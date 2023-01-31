using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    [Header("Abilities")]
    public Abilities[] abilities;
    public Unit target;
    [SerializeField] WeaponElement monsterAttackElement;
    public WeaponElement MonsterAttackElement { get { return monsterAttackElement; } }

    [SerializeField] WeaponElement monsterDefenseElement;
    public WeaponElement MonsterDefenseElement { get { return monsterDefenseElement; } }

    //Enemy Stats

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite reactSprite;
    public MonsterController monsterControl;

    public int lowHealth;
    List<Tile> monsterSpace;

    [Header("UI")]
    [SerializeField] UnitUI monsterUI;

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

        timelineFill = 30;
        timelineTypes = TimeLineTypes.EnemyUnit;
        health = maxHealth;

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

    public override void Damage()
    {
        base.Damage();
        sprite.sprite = reactSprite;
    }

    public override void Default()
    {
        base.Default();
        sprite.sprite = defaultSprite;
    }
 
  
    public void UpdateMonsterSpace(Board board)
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
        monsterControl.monsterAnimations.SetBool("death", true);
        
        isDead = true;
        AudioManager.instance.Play("MonsterDeath");
        controller.ChangeState<WinState>();
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
               
            }

            return false;
        }
    }

    public override bool ReceiveDamage(float damage)
    {
        health -= (int)damage;
        DamageEffect();
        monsterUI.CreatePopUpText(transform.position, (int)damage);

        monsterControl.monsterAnimations.SetTrigger("damage");

        if(health <= lowHealth)
        {
            monsterControl.monsterAnimations.SetTrigger("lowHealth");
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
}
