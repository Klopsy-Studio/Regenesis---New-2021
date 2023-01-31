using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    
    public bool isTraverseCalled = false;
    public int range = 5;
    int originalRange;
    public int OriginalRange { get { return originalRange; } }
    public int jumpHeight;
    protected Unit unit;
    protected Transform jumper;

    public bool moving;

    protected TimeLineTypes filter;
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        //if(unit.UnitType == UnitType.PlayerUnit)
        //{
        //    originalRange = 5;
        //}
        //else 
        //{
        //    originalRange = 10;
        //}

        jumper = transform.Find("Jumper");
    }

    private void Start()
    {
        originalRange = range;
    }

    public virtual List<Tile> GetTilesInRange(Board board, bool filterEnemies)
    {
        List<Tile> retValue = board.Search(unit.tile, ExpandSearch);
        Filter(retValue, filterEnemies);
        return retValue;
    }

    public virtual List<Tile> GetTilesInRangeWithEnemy(Board board, bool filterEnemies)
    {
        List<Tile> retValue = board.Search(unit.tile, ExpandSearchWithEnemies);
        Filter(retValue, filterEnemies);
        return retValue;
    }
    protected virtual bool ExpandSearch(Tile from, Tile to) //Conditions for units to traverse tiles
    {
        return (from.distance + 1) <= range;
    }
    protected virtual bool ExpandSearchWithEnemies(Tile from, Tile to)
    {
        return (from.distance + 1) <= range;
    }
    protected virtual void Filter(List<Tile> tiles, bool filterEnemies)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
        {
            if (!filterEnemies)
            {
                if (tiles[i].content != null)
                {
                    if (tiles[i].content.GetComponent<EnemyUnit>() == null)
                    {
                        tiles.RemoveAt(i);
                    }
                }
            }
            else
            {
                if (tiles[i].content != null || tiles[i].occupied)
                {
                    tiles.RemoveAt(i);
                }
            }
        }

    }

    public void ChangeFilter(TimeLineTypes t)
    {
        filter = t;
    }
    public virtual List<Tile> GetTilesInRangeForEnemy(Board board, bool isAttack)
    {
        List<Tile> retValue = board.Search(unit.tile, ExpandSearch);
        FilterForEnemies(retValue, isAttack);
        return retValue;
    }

    protected virtual void FilterForEnemies(List<Tile> tiles, bool isAttack)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
        {
            if (isAttack)
            {
                if (tiles[i].content != null)
                {
                    if (tiles[i].content.GetComponent<PlayerUnit>() == null)
                    {
                        tiles.RemoveAt(i);
                    }
                }
            }
            else
            {
                if (tiles[i].content != null)
                {
                    tiles.RemoveAt(i);
                }
            }
        }

    }

    public void StartSimpleTraverse(Tile tile)
    {
        StartCoroutine(SimpleTraverse(tile));
    }
    public void StartTraverse(Tile tile, Board board)
    {
        StartCoroutine(Traverse(tile, board));
    }
    public abstract IEnumerator SimpleTraverse(Tile tile); //Unit just teleports.

    public abstract IEnumerator Traverse(Tile tile, Board board); //Traverse animation

    //public void StartTraverse(Tile tile)
    //{
    //    if (!isTraverseCalled)
    //    {
    //        isTraverseCalled = true;
    //        Debug.Log("LLAMANDO A CORUTINA");
    //        StartCoroutine(Traverse(tile, board));          
    //    }
      
    //}

    public bool IsDoneMoving()
    {
        if (moving)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    protected virtual IEnumerator Turn(Directions dir)
    {
        if(unit.dir == Directions.North || unit.dir == Directions.East)
        {
            if(dir == Directions.South || dir == Directions.West)
            {
                unit.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
            else
            {
                unit.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;

            }
        }
        unit.dir = dir;

        
        yield return null;
    }

    public void ResetRange()
    {
        range = OriginalRange;
    }

    public bool CanBePushed(Directions pushDir, int pushStrength, Board board)
    {
        range = pushStrength;
        MovementRange moveRange = unit.GetComponent<MovementRange>();
        moveRange.tile = unit.tile;
        moveRange.range = pushStrength;
        moveRange.removeOrigin = true;
        List<Tile> t = moveRange.GetTilesInRange(board);
        Tile desiredTile = null;

        foreach (Tile dirTile in t)
        {
            if (unit.tile.GetDirections(dirTile) == pushDir)
            {
                desiredTile = dirTile;
            }
        }

        if (desiredTile != null && desiredTile.content == null)
        {
            return true;
        }

        else
        {
            if (desiredTile != null)
            {
                if (desiredTile.content.GetComponent<PlayerUnit>() == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }

            else
            {
                return false;
            }

        }
    }
    public virtual void PushUnit(Directions pushDir, int pushStrength, Board board)
    {
        LineAbilityRange moveRange = unit.GetComponent<LineAbilityRange>();

        moveRange.lineDir = pushDir;
        moveRange.lineLength = pushStrength;
        moveRange.stopLine = true;

        List<Tile> tiles = moveRange.GetTilesInRange(board);
        Tile desiredTile = null;

        for (int i = 0; i < tiles.Count; i++)
        {
            if(tiles[i].content == null && !tiles[i].occupied)
            {
                desiredTile = tiles[i];
            }
            else
            {
                break;
            }
        }

        if(desiredTile != null)
        {
            StartCoroutine(Traverse(desiredTile, board));

            if (unit.GetComponent<PlayerUnit>() != null)
            {
                unit.GetComponent<PlayerUnit>().Push();
            }
        }

        else
        {
            if(desiredTile != null)
            {
                if (desiredTile.content.GetComponent<PlayerUnit>() == null)
                {
                    unit.Stun();
                }
                else
                {
                    if(desiredTile.content.GetComponent<Movement>().CanBePushed(pushDir, pushStrength, board))
                    {
                        StartCoroutine(Traverse(desiredTile, board));

                        if (unit.GetComponent<PlayerUnit>() != null)
                        {
                            unit.GetComponent<PlayerUnit>().Push();
                        }
                    }
                }

            }

            else
            {
                unit.Stun();
            }
            
        }
        
    }

    public void ChangeRange(int newRange)
    {
        range = newRange;
    }
}
