using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEvent : RealTimeEvents
{
    [SerializeField] Tile tileToPlace;
    void Start()
    {
        
    }

    public override void InitialSettings()
    {
        base.InitialSettings();
        GetRandomTile();
        Board.SelectAttackTiles(tileToPlace.GetEvent2Area(Board));
    }

    public void GetRandomTile()
    {
        tileToPlace = Board.tiles[Random.Range(0, Board.tiles.Count)];
    }

    public List<Tile> GetEventTiles()
    {
        return tileToPlace.GetEvent2Area(Board);
    }

    public void ResetEvent()
    {
        GetRandomTile();
        timelineFill = 0;
    }
    public override void ApplyEffect()
    {
        List<Tile> eventTiles = GetEventTiles();

        List<Unit> units = new List<Unit>(); ;
        List<ItemElements> items = new List<ItemElements>();

        foreach(Tile t in eventTiles)
        {
            if(t.content != null)
            {
                if(t.content.GetComponent<Unit>() != null)
                {
                    if (!units.Contains(t.content.GetComponent<Unit>()))
                    {
                        units.Add(t.content.GetComponent<Unit>());
                    }
                }

                else if (t.content.GetComponent<ItemElements>() != null)
                {
                    if (!items.Contains(t.content.GetComponent<ItemElements>()))
                    {
                        items.Add(t.content.GetComponent<ItemElements>());
                    }
                }
            }

            if (t.occupied)
            {
                if (!units.Contains(battleController.enemyUnits[0]))
                {
                    units.Add(battleController.enemyUnits[0]);
                }
            }
        }
        List<Tile> allTiles = Board.tiles;

        foreach(Tile t in eventTiles)
        {
            if (allTiles.Contains(t))
            {
                allTiles.Remove(t);
            }
        }
        
        if(units.Count > 0)
        {
            foreach(Unit u in units)
            {
                if(u == battleController.enemyUnits[0])
                {
                    List<Tile> validTilesForMonster = new List<Tile>();

                    foreach(Tile t in allTiles)
                    {
                        if (t.CheckSurroundings(Board))
                        {
                            validTilesForMonster.Add(t);
                        }
                    }

                    if (validTilesForMonster.Count > 0)
                    {
                        StartCoroutine(u.GetComponent<Movement>().SimpleTraverse(validTilesForMonster[Random.Range(0, validTilesForMonster.Count)]));
                        u.GetComponent<EnemyUnit>().UpdateMonsterSpace(Board);
                    }
                }
                else
                {
                    List<Tile> validTilesForUnits = new List<Tile>();

                    foreach(Tile t in allTiles)
                    {
                        if(!t.occupied && t.content == null)
                        {
                            validTilesForUnits.Add(t);
                        }
                    }

                    if(validTilesForUnits.Count > 0)
                    {
                        StartCoroutine(u.GetComponent<Movement>().SimpleTraverse(validTilesForUnits[Random.Range(0, validTilesForUnits.Count)]));
                    }
                }
            }
        }

        if (items.Count > 0)
        {
            foreach (ItemElements u in items)
            {
                List<Tile> validTilesForItems = new List<Tile>();

                foreach (Tile t in allTiles)
                {
                    if (!t.occupied && t.content == null)
                    {
                        validTilesForItems.Add(t);
                    }
                }

                if(validTilesForItems.Count > 0)
                {
                    Tile t = validTilesForItems[Random.Range(0, validTilesForItems.Count)];

                    u.transform.position = new Vector3(t.pos.x, u.transform.position.y, t.pos.y);

                    t.content = u.gameObject;
                }
            }
        }

        timelineFill = 0;
    }
}
