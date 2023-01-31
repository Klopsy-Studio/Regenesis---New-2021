using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEvent : TimelineElements
{
    public MonsterController controller;
    public List<RangeData> rangeDisplay;
    public bool acting;
    public virtual void Apply()
    {
        StartCoroutine(Event());
    }

    public virtual IEnumerator Event()
    {
        acting = true;
        yield return null;
    }
    public override bool UpdateTimeLine()
    {
        if (timelineFill >= timelineFull)
        {
            return true;
        }

        timelineFill += fTimelineVelocity * Time.deltaTime;

        return false;
    }



    public List<Tile> GetEventTiles()
    {
        List<Tile> tiles = new List<Tile>();

        foreach(RangeData data in rangeDisplay)
        {
            AbilityRange range = data.GetOrCreateRange(data.range, this.gameObject);
            range.unit = controller.currentEnemy;

            List<Tile> repeatTiles = range.GetTilesInRange(controller.battleController.board);

            foreach(Tile t in repeatTiles)
            {
                if (!tiles.Contains(t))
                {
                    tiles.Add(t);
                }
            }
        }

        return tiles;
    }

    public void HighlightTiles(Board board)
    {
        List<Tile> tiles = GetEventTiles();
        board.SelectAttackTiles(tiles);
    }

    public void DeactivateTiles(Board board)
    {
        List<Tile> tiles = GetEventTiles();
        board.DeSelectDefaultTiles(tiles);
    }
}
