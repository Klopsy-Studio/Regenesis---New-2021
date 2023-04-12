using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEvent : RealTimeEvents
{
    [SerializeField] Tile tileToPlace;


    public override void InitialSettings()
    {
        base.InitialSettings();
        GetRandomTile();
        GetEventTiles();
    }

    public void GetRandomTile()
    {
        tileToPlace = board.tiles[Random.Range(0, board.tiles.Count)];
    }

    public List<Tile> GetEventTiles()
    {
        return tileToPlace.GetEvent2Area(board);
    }

    public void ResetEvent()
    {
        GetRandomTile();
        tileToPlace.GetEvent2Area(board);
        timelineFill = 0;
    }
    public override void ApplyEffect()
    {
        playing = true;
        StartCoroutine(Effect());
        //List<Tile> eventTiles = GetEventTiles();

        //List<Unit> units = new List<Unit>(); ;
        //List<ItemElements> items = new List<ItemElements>();

        
        //foreach(Tile t in eventTiles)
        //{
        //    t.DisableCracks();
        //    if(t.content != null)
        //    {
        //        if(t.content.GetComponent<Unit>() != null)
        //        {
        //            if (!units.Contains(t.content.GetComponent<Unit>()))
        //            {
        //                units.Add(t.content.GetComponent<Unit>());
        //            }
        //        }

        //        else if (t.content.GetComponent<ItemElements>() != null)
        //        {
        //            if (!items.Contains(t.content.GetComponent<ItemElements>()))
        //            {
        //                items.Add(t.content.GetComponent<ItemElements>());
        //            }
        //        }
        //    }

        //    if (t.occupied)
        //    {
        //        if (!units.Contains(battleController.enemyUnits[0]))
        //        {
        //            units.Add(battleController.enemyUnits[0]);
        //        }
        //    }
        //}
        //List<Tile> allTiles = board.tiles;

        //foreach(Tile t in eventTiles)
        //{
        //    if (allTiles.Contains(t))
        //    {
        //        allTiles.Remove(t);
        //    }
        //}
        
        //if(units.Count > 0)
        //{
        //    foreach(Unit u in units)
        //    {

        //        if(u == battleController.enemyUnits[0])
        //        {
        //            List<Tile> validTilesForMonster = new List<Tile>();

        //            foreach(Tile t in allTiles)
        //            {
        //                if (t.CheckSurroundings(board))
        //                {
        //                    validTilesForMonster.Add(t);
        //                }
        //            }

        //            if (validTilesForMonster.Count > 0)
        //            {
        //                StartCoroutine(u.GetComponent<Movement>().SimpleTraverse(validTilesForMonster[Random.Range(0, validTilesForMonster.Count)]));
        //                u.GetComponent<EnemyUnit>().UpdateMonsterSpace(board);
        //            }
        //        }
        //        else
        //        {
        //            List<Tile> validTilesForUnits = new List<Tile>();

        //            foreach (Tile t in allTiles)
        //            {
        //                if (!t.occupied && t.content == null)
        //                {
        //                    //Set animation for travel
        //                    validTilesForUnits.Add(t);
        //                }
        //            }

        //            if (validTilesForUnits.Count > 0)
        //            {
        //                StartCoroutine(u.GetComponent<Movement>().SimpleTraverse(validTilesForUnits[Random.Range(0, validTilesForUnits.Count)]));
        //            }
        //        }
        //    }
        //}

        //if (items.Count > 0)
        //{
        //    foreach (ItemElements u in items)
        //    {
        //        List<Tile> validTilesForItems = new List<Tile>();

        //        foreach (Tile t in allTiles)
        //        {
        //            if (!t.occupied && t.content == null)
        //            {
        //                validTilesForItems.Add(t);
        //            }
        //        }

        //        if(validTilesForItems.Count > 0)
        //        {
        //            Tile t = validTilesForItems[Random.Range(0, validTilesForItems.Count)];

        //            u.transform.position = new Vector3(t.pos.x, u.transform.position.y, t.pos.y);

        //            t.content = u.gameObject;
        //        }
        //    }
        //}

        //timelineFill = 0;
    }



    IEnumerator Effect()
    {
        List<Tile> eventTiles = GetEventTiles();

        List<Unit> units = new List<Unit>(); ;
        List<ItemElements> items = new List<ItemElements>();


        foreach (Tile t in eventTiles)
        {
            t.DisableCracks();
            if (t.content != null)
            {
                if (t.content.GetComponent<Unit>() != null)
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
        List<Tile> allTiles = board.tiles;

        foreach (Tile t in eventTiles)
        {
            if (allTiles.Contains(t))
            {
                allTiles.Remove(t);
            }
        }
        List<Tile> validTilesForUnits = new List<Tile>();

        foreach (Tile t in allTiles)
        {
            if (!t.occupied && t.content == null)
            {
                //Set animation for travel
                validTilesForUnits.Add(t);
            }
        }

        if (units.Count > 0)
        {
            foreach (Unit u in units)
            {
                if (u == battleController.enemyUnits[0])
                {
                    continue;
                }
                else
                {
                    if(u.GetComponent<PlayerUnit>()!= null)
                    {
                        u.GetComponent<PlayerUnit>().animations.unitAnimator.SetTrigger("drop");
                    }
                    
                }
            }

            yield return new WaitForSeconds(2f);

            foreach (Unit u in units)
            {
                if (u == battleController.enemyUnits[0])
                {
                    continue;
                }
                else
                {
                    Tile t = validTilesForUnits[Random.Range(0, validTilesForUnits.Count)];
                    StartCoroutine(u.GetComponent<Movement>().SimpleTraverse(t));

                    validTilesForUnits.Remove(t);
                }
                
            }
        }

        

        //Deactivating it for items
        //if (items.Count > 0)
        //{
        //    foreach (ItemElements u in items)
        //    {
        //        List<Tile> validTilesForItems = new List<Tile>();

        //        foreach (Tile t in allTiles)
        //        {
        //            if (!t.occupied && t.content == null)
        //            {
        //                validTilesForItems.Add(t);
        //            }
        //        }

        //        if (validTilesForItems.Count > 0)
        //        {
        //            Tile t = validTilesForItems[Random.Range(0, validTilesForItems.Count)];

        //            u.transform.position = new Vector3(t.pos.x, u.transform.position.y, t.pos.y);

        //            t.content = u.gameObject;
        //        }
        //    }
        //}

        timelineFill = 0;

        playing = false;

    }
}
