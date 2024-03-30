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
    }



    IEnumerator Effect()
    {
        battleController.SelectTile(tileToPlace.pos);
        yield return new WaitForSeconds(0.5f);

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
                    battleController.SelectTile(u.tile.pos);

                    if(u.TryGetComponent<PlayerUnit>(out PlayerUnit p))
                    {
                        p.animations.unitAnimator.SetBool("allowDeath", false);

                        p.animations.unitAnimator.SetTrigger("drop");
                    }

                    if (u.TryGetComponent<EnemyUnit>(out EnemyUnit e))
                    {
                        e.monsterControl.monsterAnimations.SetTrigger("drop");
                    }

                    yield return new WaitForSeconds(1.5f);
                }
            }

            foreach (Unit u in units)
            {
                if (u == battleController.enemyUnits[0])
                {
                    continue;
                }
                else
                {
                    if (u.TryGetComponent<PlayerUnit>(out PlayerUnit p))
                    {
                        p.animations.unitAnimator.SetTrigger("appear");
                    }

                    if (u.TryGetComponent<EnemyUnit>(out EnemyUnit e))
                    {
                        e.monsterControl.monsterAnimations.SetTrigger("appear");
                    }

                    Tile t = validTilesForUnits[Random.Range(0, validTilesForUnits.Count)];
                    StartCoroutine(u.GetComponent<Movement>().SimpleTraverse(t));
                    battleController.SelectTile(t.pos);
                    validTilesForUnits.Remove(t);
                    yield return new WaitForSeconds(1.5f);
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
