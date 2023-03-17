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

    public override bool ReceiveDamage(int damage, bool isCritical)
    {
        health -= (int)damage;
        //DamageEffect();
        monsterUI.CreatePopUpText(transform.position + new Vector3(0, 1, 0), (int)damage, isCritical);

        monsterControl.monsterAnimations.SetTrigger("damage");

        if (health <= lowHealth)
        {
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
   
}

public enum TypeOfEnemy
{
    Big, Small
}
