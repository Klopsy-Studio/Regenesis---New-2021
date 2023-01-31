using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateSideRange : AbilityRange
{
    public Directions alternateSideDir;
    public int alternateSideReach;
    public int alternateSideLength;
    public override List<Tile> GetTilesInRange(Board board)
    {
        List<Tile> retValue = new List<Tile>();

        switch (alternateSideDir)
        {
            case Directions.North:

                for (int i = -alternateSideLength; i < alternateSideLength + 1; i+=2)
                {
                    if (GetTileInPosition(new Point(i, alternateSideLength), board) != null)
                    {
                        retValue.Add(GetTileInPosition(new Point(i, alternateSideLength), board));
                    }
                }
                break;
            case Directions.East:
                for (int i = -alternateSideLength; i < alternateSideLength + 1; i+=2)
                {
                    if (GetTileInPosition(new Point(-alternateSideLength, i), board) != null)
                    {
                        retValue.Add(GetTileInPosition(new Point(-alternateSideLength, i), board));
                    }
                }
                break;
            case Directions.South:
                for (int i = -alternateSideLength; i < alternateSideLength + 1; i+=2)
                {
                    if (GetTileInPosition(new Point(i, -alternateSideLength), board) != null)
                    {
                        retValue.Add(GetTileInPosition(new Point(i, -alternateSideLength), board));
                    }
                }
                break;
            case Directions.West:
                for (int i = -alternateSideLength; i < alternateSideLength + 1; i+=2)
                {
                    if (GetTileInPosition(new Point(alternateSideLength, i), board) != null)
                    {
                        retValue.Add(GetTileInPosition(new Point(alternateSideLength, i), board));
                    }
                }
                break;
            default:
                break;
        }

        return retValue;
    }

    public override void AssignVariables(RangeData data)
    {
        alternateSideDir = data.alternateSideDir;
        alternateSideReach = data.alternateSideReach;
        alternateSideLength = data.alternateSideLength;
    }
}
