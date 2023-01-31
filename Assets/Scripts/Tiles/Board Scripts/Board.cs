using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Board : MonoBehaviour //Adjust to new level creation system. Examples: Load our level design, illumination 
{
    [SerializeField] GameObject tilePrefab;
    [Header("Tiles")]
    [SerializeField] GameObject[] desertTilesPrefab;
    [SerializeField] GameObject[] placeholderTilesPrefab;
    [SerializeField] GameObject[] nonplayableTilesPrefab;
    [Space]

    [Header("Props")]
    [SerializeField] GameObject[] cityPropsPrefabs;
    [SerializeField] GameObject[] desertPropsPrefabs;
    [SerializeField] GameObject[] parkPropsPrefabs;

    [Space]
    [Header("Obstacles")]
    [SerializeField] GameObject regularObstaclePrefab;
    [SerializeField] Transform battleController;

    public Dictionary<Point, Tile> playableTiles = new Dictionary<Point, Tile>();
    public Dictionary<Point, Tile> nonPlayableTiles = new Dictionary<Point, Tile>();

    public List<GameObject> obstacles;
    Point[] dirs = new Point[4]
    {
        new Point(0, 1),
        new Point(0, -1),
        new Point(1, 0),
        new Point(-1, 0)
    };

    public Color selectedTileColor = new Color(0, 1, 1, 1);

    public bool toggleTileActivation;
    public void Load(LevelData data)
    {
        for (int i = 0; i < data.tiles.Count; i++)
        {
            SpawnTile(data, i);
        }

        for (int i = 0; i < data.props.Count; i++)
        {
            SpawnProp(data, i);
        }

        if(data.levelModel != null)
        {
            Instantiate(data.levelModel);
        }
    }


    public void ActivateTileSelection()
    {
        if (!toggleTileActivation) return;
        foreach(Tile t in playableTiles.Values)
        {
            t.ActivateTileSelection();
        }
    }
    public void ActivateTileSelectionToggle()
    {
        foreach (Tile t in playableTiles.Values)
        {
            t.ActivateTileSelection();
        }
    }
    public void DeactivateTileSelection()
    {
        
        foreach (Tile t in playableTiles.Values)
        {
            t.DeactivateTileSelection();
        }
    }
    Prop SpawnProp(LevelData data, int i)
    {
        switch (data.propData[i].type)
        {
            case PropType.City:
                GameObject instance = Instantiate(cityPropsPrefabs[data.propData[i].typeIndex]) as GameObject;
                instance.transform.parent = battleController.transform;
                Prop p = instance.GetComponent<Prop>();
                p.Load(data.props[i]);

                if (p.data.occupiesSpace)
                {
                    Tile t = GetTile(p.pos);
                    t.content = p.gameObject;
                }
                return p;
            case PropType.Desert:
                instance = Instantiate(desertPropsPrefabs[data.propData[i].typeIndex]) as GameObject;
                instance.transform.parent = battleController.transform;
                p = instance.GetComponent<Prop>();
                p.data = data.propData[i];
                p.Load(data.props[i]);

                if (p.data.occupiesSpace)
                {
                    Tile t = GetTile(p.pos);
                    t.content = p.gameObject;
                }
                return p;
            case PropType.Park:
                instance = Instantiate(parkPropsPrefabs[data.propData[i].typeIndex]) as GameObject;
                instance.transform.parent = battleController.transform;
                p = instance.GetComponent<Prop>();
                
                p.Load(data.props[i]);

                if (p.data.occupiesSpace)
                {
                    Tile t = GetTile(p.pos);
                    t.content = p.gameObject;
                }
                return p;
            default:
                return null;
        }
    }

    int index = 0;
    Tile SpawnTile(LevelData data, int i)
    {
        GameObject instance = Instantiate(tilePrefab) as GameObject;
        instance.transform.parent = battleController.transform;
        Tile t = instance.GetComponent<Tile>();
        t.Load(data.tiles[i]);

        t.data = data.tileData[i];
        t.tileSprite.sprite = data.sprites[i];


        if (t.data.isPlayable)
        {
            playableTiles.Add(t.pos, t);
        }
        else
        {
            nonPlayableTiles.Add(t.pos, t);
        }

        return t;

    }

    public Dictionary<Point, Tile> GetPlayableDictionary()
    {
        return playableTiles;
    }

    public Dictionary<Point, Tile> GetNonPlayableDictionary()
    {
        return nonPlayableTiles;
    }

    public List<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile) //Func<Tile, Tile, bool> It is a generic that needs two Tile parameters and it
    {                                                                    // returns a bool. Its function is to detect that the distance to the second tile is within the range
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);
        ClearSearch();
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        start.distance = 0;
        checkNow.Enqueue(start);
        while (checkNow.Count > 0)
        {
            Tile t = checkNow.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                Tile next = GetTile(t.pos + dirs[i]); //Here we are checking the tiles next to the current tile
                if (next == null || next.distance <= t.distance + 1)
                    continue;
                if (addTile(t, next))
                {
                    next.distance = t.distance + 1;
                    next.prev = t;
                    checkNext.Enqueue(next);
                    retValue.Add(next);
                }

            }

            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkNext);

        }
        return retValue;
    }

    void ClearSearch() //Clean the result of previous search
    {
        foreach (Tile t in playableTiles.Values)
        {
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }

    public Tile GetTile(Point p)
    {
        return playableTiles.ContainsKey(p) ? playableTiles[p] : null;
    }

    void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }


    public void SelectMovementTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
        {
            tiles[i].ChangeTile(tiles[i].movementColor);
            tiles[i].selected = true;
        }
    }

    public void SelectAbilityTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
        {
            tiles[i].ChangeTile(tiles[i].abilityColor);
            tiles[i].selected = true;
        }
    }
    public void SelectAttackTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
        {
            tiles[i].ChangeTile(tiles[i].attackColor);
            tiles[i].selected = true;
        }
    }
    public void DeSelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
        {
            tiles[i].ChangeTile(tiles[i].previousColor);
            tiles[i].selected = false;
        }
    }
    public void DeSelectDefaultTiles(List<Tile> tiles)
    {
        //for (int i = tiles.Count - 1; i >= 0; --i)
        //{
        //    tiles[i].ChangeTile(tiles[i].defaultTileColor);
        //    tiles[i].selected = false;
        //}

        foreach(Tile t in tiles)
        {
            t.ChangeTileToDefault();
            t.selected = false;
        }
    }
}
