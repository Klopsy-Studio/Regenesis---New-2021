using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAbilityRange : AbilityRange
{
    public int lineLength = 2;
    public Directions lineDir;
    public bool stopLine = false;
    public int lineOffset = 0;
    public bool monsterUse = false;
    public override List<Tile> GetTilesInRange(Board board)
    {
        Point startPos = unit.tile.pos;
        List<Tile> retValue  = new List<Tile>();

        switch (lineDir)
        {
            case Directions.North:
                for (int i = 1; i < lineLength+1; i++)
                {
                    if(lineOffset >= i)
                    {
                        continue;
                    }
                    else
                    {
                        if (board.GetTile(startPos + new Point(0, i)) != null)
                        {
                            Tile t = board.GetTile(startPos + new Point(0, i));
                            retValue.Add(board.GetTile(startPos + new Point(0, i)));

                            if (stopLine)
                            {
                                if (t.content != null)
                                {
                                    break;
                                }
                                if (t.occupied)
                                {
                                    if (!monsterUse)
                                    {
                                        break;
                                    }
                                }
                                
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                break;
            case Directions.East:
                for (int i = 1; i < lineLength+1; i++)
                {
                    if (lineOffset >= i)
                    {
                        continue;
                    }
                    else
                    {
                        if (board.GetTile(startPos + new Point(-i, 0)) != null)
                        {
                            Tile t = board.GetTile(startPos + new Point(-i, 0));
                            retValue.Add(t);

                            if (stopLine)
                            {
                                if (t.content != null)
                                {
                                    if (monsterUse)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (t.occupied || t.content != null)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                break;
            case Directions.South:
                for (int i = 1; i < lineLength+1; i++)
                {
                    if (lineOffset >= i)
                    {
                        continue;
                    }
                    else
                    {
                        if (board.GetTile(startPos + new Point(0, -i)) != null)
                        {
                            Tile t = board.GetTile(startPos + new Point(0, -i));

                            retValue.Add(t);

                            if (stopLine)
                            {
                                if (t.content != null)
                                {
                                    if (monsterUse)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (t.occupied || t.content != null)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                break;
            case Directions.West:
                for (int i = 1; i < lineLength+1; i++)
                {
                    if (lineOffset >= i)
                    {
                        continue;
                    }
                    else
                    {
                        if (board.GetTile(startPos + new Point(i, 0)) != null)
                        {
                            Tile t = board.GetTile(startPos + new Point(i, 0));
                            retValue.Add(t);
                            if (stopLine)
                            {
                                if (t.content != null)
                                {
                                    if (monsterUse)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (t.occupied || t.content != null)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        else
                        {
                            break;
                        }
                    }
                }
                break;
            default:
                break;
        }

        return retValue;

    }

    public void ChangeDirection(Directions dir)
    {
        lineDir = dir;
    }

    public override void AssignVariables(RangeData rangeData)
    {
        lineDir = rangeData.lineDir;
        lineLength = rangeData.lineLength;
        stopLine = rangeData.stopLine;
        lineOffset = rangeData.lineOffset;
        monsterUse = rangeData.monsterUse;
    }
}
