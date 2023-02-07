using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionUnit : EnemyUnit
{
    public MonsterController parent;
    [SerializeField] RangeData range;


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
        parent.minionsInGame.Remove(this);
        controller.enemyUnits.Remove(this);

        tile.content = null;

        Destroy(gameObject);
    }
    public List<Tile> GetMinionAttackArea(Board board)
    {
        AbilityRange ability = range.GetOrCreateRange(range.range, this.gameObject);
        ability.unit = this;
        List<Tile> dirtyTiles = ability.GetTilesInRange(board);
        List<Tile> validTiles = new List<Tile>();

        foreach(Tile t in dirtyTiles)
        {
            if (!t.occupied)
            {
                validTiles.Add(t);
            }
        }

        return validTiles;
    }

}
