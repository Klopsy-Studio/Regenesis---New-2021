using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionUnit : EnemyUnit
{
    [SerializeField] RangeData range;
    public override void UpdateMonsterSpace(Board board)
    {
        
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
