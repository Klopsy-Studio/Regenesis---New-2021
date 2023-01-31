using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAbilityRange : AbilityRange
{
    public int monsterReach = 2;
    public override List<Tile> GetTilesInRange(Board board)
    {
        List<Tile> retValue = new List<Tile>();


        if (GetTileInPosition(new Point(monsterReach, 0), board) != null)
        {
            retValue.Add(GetTileInPosition(new Point(monsterReach, 0), board));
        }

        if (GetTileInPosition(new Point(monsterReach, 1), board) != null)
        {
            retValue.Add(GetTileInPosition(new Point(monsterReach, 1), board));
        }

        if (GetTileInPosition(new Point(monsterReach, -1), board) != null)
        {
            retValue.Add(GetTileInPosition(new Point(monsterReach, -1), board));
        }

        if (GetTileInPosition(new Point(-monsterReach, 0), board) != null)
        {
            retValue.Add(GetTileInPosition(new Point(-monsterReach, 0), board));
        }

        if (GetTileInPosition(new Point(-monsterReach, 1), board) != null)
        {
            retValue.Add(GetTileInPosition(new Point(-monsterReach, 1), board));
        }

        if (GetTileInPosition(new Point(-monsterReach, -1), board) != null)
        {
            retValue.Add(GetTileInPosition(new Point(-monsterReach, -1), board));
        }

        if (GetTileInPosition(new Point(0, monsterReach), board) != null)
        {
            retValue.Add(GetTileInPosition(new Point(0, monsterReach), board));
        }

        if (GetTileInPosition(new Point(1, monsterReach), board) != null)
        {
            retValue.Add(GetTileInPosition(new Point(1, monsterReach), board));
        }

        if (GetTileInPosition(new Point(-1, monsterReach), board) != null)
        {
            retValue.Add(GetTileInPosition(new Point(-1, monsterReach), board));
        }

        if (GetTileInPosition(new Point(0, -monsterReach), board) != null)
        {
            retValue.Add(GetTileInPosition(new Point(0, -monsterReach), board));
        }

        if (GetTileInPosition(new Point(1, -monsterReach), board) != null)
        {
            retValue.Add(GetTileInPosition(new Point(1, -monsterReach), board));
        }

        if (GetTileInPosition(new Point(-1, -monsterReach), board) != null)
        {
            retValue.Add(GetTileInPosition(new Point(-1, -monsterReach), board));
        }
        return retValue;
    }



    protected Tile GetTileInPosition(Point pos, Board board)
    {
        if (board.GetTile(unit.currentPoint + pos) != null)
        {
            return board.GetTile(unit.currentPoint + pos);
        }
        else
        {
            return null;
        }
    }
}
