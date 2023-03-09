using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareAbilityRange : AbilityRange
{
    public int squareReach = 1;
    public bool removeOrigin;
    Point initialPosition;
    public override List<Tile> GetTilesInRange(Board board)
    {
        List<Tile> retValue = new List<Tile>();

        if (!removeOrigin)
        {
            retValue.Add(GetTileInPosition(new Point(0, 0), board));
        }

        for (int x = -squareReach; x < squareReach + 1; x++)
        {
            for (int y = -squareReach; y < squareReach + 1; y++)
            {
                if (IsTileValid(board, x, y))
                {
                    retValue.Add(board.GetTile(new Point(unit.currentPoint.x + x, unit.currentPoint.y + y)));
                }
            }
        }

        return retValue;
    }

    public override List<Tile> GetTilesInRangeWithoutUnit(Board board, Point initialPos)
    {
        List<Tile> retValue = new List<Tile>();

        initialPosition = initialPos;

        if (!removeOrigin)
        {
            retValue.Add(board.GetTile(new Point(initialPos.x, initialPos.y)));
        }

        for (int x = -squareReach; x < squareReach + 1; x++)
        {
            for (int y = -squareReach; y < squareReach + 1; y++)
            {
                if (IsTileValidWithOutUnit(initialPos, board, x, y))
                {
                    retValue.Add(board.GetTile(new Point(initialPos.x + x, initialPos.y + y)));
                }
            }
        }

        return retValue;
    }

    public new Tile GetTileInPosition(Point pos, Board board)
    {
        if(unit != null)
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

        else
        {
            if (board.GetTile(initialPosition+pos) != null)
            {
                return board.GetTile(initialPosition + pos);
            }
            else
            {
                return null;
            }
        }
        
    }

    public bool IsTileValidWithOutUnit(Point initialPos, Board board, int offsetX, int offsetY)
    {
        if (board.GetTile(new Point(initialPos.x + offsetX, initialPos.y + offsetY)) != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsTileValid(Board board, int offsetX, int offsetY)
    {
        if (board.GetTile(new Point(unit.currentPoint.x + offsetX, unit.currentPoint.y + offsetY)) != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public override void AssignVariables(RangeData rangeData)
    {
        squareReach = rangeData.squareReach;
        removeOrigin = rangeData.removeOrigin;
    }
}
