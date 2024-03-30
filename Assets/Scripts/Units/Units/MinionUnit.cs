using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionUnit : EnemyUnit
{
    public MonsterController parent;
    public RangeData movementRange;
    [SerializeField] List<RangeData> eventAttackRange;

    public string evolvedName;
    public string evolvedHeader;
    public string evolvedDescription;

    protected override void Start()
    {
        Match();
        SetInitVelocity();
        popUps = GetComponent<UnitUI>();
        originalTimeStunned = timeStunned;
        timelineTypes = TimeLineTypes.EnemyUnit;
        health = maxHealth;

        SetOriginalValues();
    }
    public override void UpdateMonsterSpace(Board board)
    {
        
    }

    public override void Die()
    {
        controller.timelineElements.Remove(this);
        controller.unitsInGame.Remove(this);
        parent.minionsInGame.Remove(this);
        controller.enemyUnits.Remove(this);
        elementEnabled = false;
        tile.content = null;
        monsterControl.monsterAnimations.SetBool("death", true);
        Destroy(gameObject, 3f);
    }
    public List<Tile> GetMinionAttackArea(Board board)
    {
        List<Tile> validTiles = new List<Tile>();
        foreach (RangeData range in eventAttackRange)
        {
            AbilityRange ability = range.GetOrCreateRange(range.range, this.gameObject);
            ability.unit = this;
            List<Tile> dirtyTiles = ability.GetTilesInRange(board);

            foreach (Tile t in dirtyTiles)
            {
                if (!t.occupied && !validTiles.Contains(t))
                {
                    validTiles.Add(t);
                }
            }

        }

        return validTiles;
    }


    public override bool ReceiveDamage(int damage, bool isCritical)
    {
        health -= (int)damage;
        DamageEffect();
        List<Modifier> trashModifiers = new List<Modifier>();
        if (debuffModifiers.Count > 0)
        {
            foreach (Modifier m in debuffModifiers)
            {
                if (m.modifierType == TypeOfModifier.Defense)
                {
                    defense = originalDefense;
                    trashModifiers.Add(m);
                }
            }
        }


        if (trashModifiers.Count > 0)
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
