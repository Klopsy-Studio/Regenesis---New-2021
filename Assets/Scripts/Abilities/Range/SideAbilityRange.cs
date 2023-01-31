using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideAbilityRange : AbilityRange
{
    public Directions sideDir;
    public int sideReach;
    public int sideLength;
    public override List<Tile> GetTilesInRange(Board board)
    {
        List<Tile> retValue = new List<Tile>();

        switch (sideDir)
        {
            case Directions.North:

                for (int i = -sideLength; i < sideLength + 1; i++)
                {
                    if (GetTileInPosition(new Point(i, sideReach), board) != null)
                    {
                        retValue.Add(GetTileInPosition(new Point(i, sideReach), board));
                    }
                }
                break;
            case Directions.East:
                for (int i = -sideLength; i < sideLength + 1; i++)
                {
                    if (GetTileInPosition(new Point(-sideReach, i), board) != null)
                    {
                        retValue.Add(GetTileInPosition(new Point(-sideReach, i), board));
                    }
                }
                break;
            case Directions.South:
                for (int i = -sideLength; i < sideLength+1; i++)
                {
                    if (GetTileInPosition(new Point(i, -sideReach), board) != null)
                    {
                        retValue.Add(GetTileInPosition(new Point(i, -sideReach), board));
                    }
                }
                break;
            case Directions.West:
                for (int i = -sideLength; i < sideLength+1; i++)
                {
                    if (GetTileInPosition(new Point(sideReach, i), board) != null)
                    {
                        retValue.Add(GetTileInPosition(new Point(sideReach, i), board));
                    }
                }
                break;
            default:
                break;
        }

        return retValue;
    }
    public void ChangeDirections(Tile t)
    {
        sideDir = unit.tile.GetDirections(t);
    }


    public override void AssignVariables(RangeData rangeData)
    {
        sideDir = rangeData.sideDir;
        sideReach = rangeData.sideReach;
        sideLength = rangeData.sideLength;
    }
}
