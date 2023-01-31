using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityRange : MonoBehaviour
{
    public int horizontal = 1;
    public int vertical = int.MaxValue;
    public virtual bool directionOriented { get { return false; } }
    public Unit unit;

    public Point startPos;
    public abstract List<Tile> GetTilesInRange(Board board);

    protected Tile GetTileInPosition(Point pos, Board board)
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
            if (board.GetTile(startPos + pos) != null)
            {
                return board.GetTile(startPos + pos);
            }
            else
            {
                return null;
            }
        }
       
    }

    public virtual void AssignVariables(RangeData rangeData)
    {
        
    }

    public virtual List<Tile> GetTilesInRangeWithoutUnit(Board board, Point initialPos)
    {
        return null;
    }

    public void SetStartPos(Point p)
    {
        startPos = p;
    }

}
