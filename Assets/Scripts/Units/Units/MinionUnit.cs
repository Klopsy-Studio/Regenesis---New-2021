using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionUnit : EnemyUnit
{
    public MonsterController parent;
    public RangeData movementRange;
    [SerializeField] List<RangeData> eventAttackRange;


    protected override void Start()
    {
        Match();
        SetInitVelocity();
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

        Destroy(gameObject);
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

}
