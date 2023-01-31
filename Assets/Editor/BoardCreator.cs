using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BoardCreator : MonoBehaviour
{
    public TileSpawner spawner;
    public Rect texturePosition;
    public Texture notPlayableTexture;
    [HideInInspector] public Sprite spriteToSpawn;
    public GameObject tilePrefab;
    public GameObject propToSpawn;
    [SerializeField] GameObject tileSelectionIndicatorPrefab;

    Transform marker
    {
        get
        {
            if (_marker == null)
            {
                GameObject instance = Instantiate(tileSelectionIndicatorPrefab) as GameObject;
                _marker = instance.transform;
            }
            return _marker;
        }

    }

    Transform _marker;

    [HideInInspector] public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
    public List<Tile> tilesScript = new List<Tile>();

    [HideInInspector] public Dictionary<Point, Prop> props = new Dictionary<Point, Prop>();
    [HideInInspector] public List<PropData> propData = new List<PropData>();

    [Header("Spawn Points")]
    [SerializeField] int howManyPlayers = 2;
    [SerializeField] int howManyEnemies = 1;
    [SerializeField] GameObject playerSpawnPrefab;
    [SerializeField] Dictionary<Point, GameObject> playerSpawnDictionary = new Dictionary<Point, GameObject>();
    [SerializeField] GameObject enemySpawnPrefab;
    [SerializeField] Dictionary<Point, GameObject> enemySpawnDictionary = new Dictionary<Point, GameObject>();

    [Header("Tile Variables")]
    public List<Point> playerSpawnPoints = new List<Point>();
    public List<Point> enemySpawnPoints = new List<Point>();
    int width = 10;
    int depth = 10;
    int height = 8;
    [SerializeField] Point pos;
    [SerializeField] LevelData levelData;


    //WE HAVE TO ADD A NEW METHOD TO UPDATE THE DICTIONARY TILES WHEN WE MOVE A TILE, TO SAVE IT LATER.

    [Header("Mission Data Variables")]
    [SerializeField] private int position;
    [SerializeField] public int rank;
    [SerializeField] public string missionName;
    [SerializeField] private Type missionType;

    [Header("Environment information")]
    [SerializeField] public Zone zone;
    [SerializeField] public Hazard hazard;
    [SerializeField] public List<string> otherCreatures;

    [Header("Rewards")]
    [SerializeField] public int money;
    [SerializeField] public List<string> items;

    [Header("Board Spawn")]
    public ThingToSpawn thingToSpawn;
    public TileType TypeOfTile;
    public TileClass classOfSprite;
    public PropType TypeOfProp;
    
    public bool makeTilePlayable;
    public bool doesPropOccupySpace;
    public bool canSpawn;



    #region Tiles

    public void GrowArea()
    {
        Rect r = RandomRect();
        GrowRect(r);
    }
    public void ShrinkArea()
    {
        Rect r = RandomRect();
        ShrinkRect(r);
    }

    Rect RandomRect()
    {
        int x = UnityEngine.Random.Range(0, width);
        int y = UnityEngine.Random.Range(0, depth);
        int w = UnityEngine.Random.Range(1, width - x + 1);
        int h = height;/* UnityEngine.Random.Range(1, depth - y + 1);*/
        return new Rect(x, y, w, h);
    }

    void GrowRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                GrowSingle(p);
            }
        }
    }

    void ShrinkRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                ShrinkSingle(p);
            }
        }
    }

    Tile Create()
    {
        GameObject instance = Instantiate(tilePrefab) as GameObject;
        instance.transform.parent = transform;
        Tile t = instance.GetComponent<Tile>();
        t.tileSprite.sprite = spriteToSpawn;
        return t;
    }

    Tile GetOrCreate(Point p)
    {
        if (tiles.ContainsKey(p))
            return tiles[p];

        Tile t = Create();
        t.Load(p, 0);
        tiles.Add(p, t);
        tilesScript.Add(t);
        return t;
    }
    public void ChangeTileToSpawn(Sprite sprite)
    {
        spriteToSpawn = sprite;
    }
    public Tile LoadTileBoard(int index)
    {
        switch (levelData.tileData.ToArray()[index].tileType)
        {
            case TileType.Placeholder:
                return LoadTile(spawner.placeholderTiles, index);
            case TileType.Desert:
                return LoadTile(spawner.desertTiles, index);
            case TileType.NonPlayable:
                return LoadTile(spawner.nonPlayableTiles, index);
            case TileType.City:
                return LoadTile(spawner.test, index);
            default:
                return null;
        }
    }

    public Tile LoadTile(Sprite[] sprites, int index)
    {
        GameObject instance = Instantiate(tilePrefab);
        Tile t = instance.GetComponent<Tile>();
        t.tileSprite.sprite = levelData.sprites[index];
        t.data.isPlayable = levelData.tileData[index].isPlayable;
        instance.transform.parent = transform;

        return t;
    }
    public void AddObstacle()
    {
        //Tile t = GetOrCreate(pos);

        //if(t.content == null)
        //{
        //    GameObject instance = Instantiate(obstaclePrefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
        //    instance.transform.parent = transform;
        //    t.content = instance;
        //    tilesObstacles.Add(instance);
        //}
    }
    void GrowSingle(Point p)
    {
        Tile t = GetOrCreate(p);
        t.data.isPlayable = makeTilePlayable;
        if (t.height < height)
            t.Grow();
    }

    void ShrinkSingle(Point p)
    {
        if (!tiles.ContainsKey(p))
        {
            Debug.Log("No key");
            return;
        }


        Tile t = tiles[p];
        tiles.Remove(p);
        tilesScript.Remove(t);
        DestroyImmediate(t.gameObject);
    }

    public void Grow()
    {
        GrowSingle(pos);
    }
    public void Shrink()
    {
        ShrinkSingle(pos);
    }


    #endregion

    #region Props

    public void SpawnProp()
    {
        CreateProp(pos);
    }
    public void CreateProp(Point p)
    {
        Prop newProp = GetOrCreateProp(p);

        newProp.data.occupiesSpace = doesPropOccupySpace;
    }

    public void ShrinkProp(Point p)
    {
        if (!props.ContainsKey(p))
        {
            Debug.Log("No hay Prop");
            return;
        }

        Prop newProp = props[p];
        props.Remove(p);
        propData.Remove(newProp.data);
        DestroyImmediate(newProp.gameObject);
    }
    public void DeleteProp()
    {
        ShrinkProp(pos);
    }

    public void ChangePropToSpawn(GameObject newProp)
    {
        propToSpawn = newProp;
    }

    public Prop LoadPropBoard(int index)
    {
        switch (levelData.propData[index].type)
        {
            case PropType.City:
                GameObject instance = Instantiate(spawner.cityProps[levelData.propData[index].typeIndex]);
                Prop p = instance.GetComponent<Prop>();
                p.data.occupiesSpace = levelData.propData[index].occupiesSpace;
                instance.transform.parent = transform;

                return p;
            case PropType.Desert:
                instance = Instantiate(spawner.desertProps[levelData.propData[index].typeIndex]);
                p = instance.GetComponent<Prop>();
                p.data.occupiesSpace = levelData.propData[index].occupiesSpace;
                instance.transform.parent = transform;

                return p;
            case PropType.Park:
                instance = Instantiate(spawner.parkProps[levelData.propData[index].typeIndex]);
                p = instance.GetComponent<Prop>();
                p.data.occupiesSpace = levelData.propData[index].occupiesSpace;
                instance.transform.parent = transform;
                return p;
            default:
                return null;
        }
    }

    Prop GetOrCreateProp(Point p)
    {
        if (props.ContainsKey(p))
            return props[p];

        Prop newProp = CreateProp();
        newProp.Load(p, 7);
        props.Add(p, newProp);
        propData.Add(newProp.data);
        return newProp;
    }

    Prop CreateProp()
    {
        GameObject instance = Instantiate(propToSpawn) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Prop>();
    }

    #endregion

    #region Marker
    public void UpdateMarker()
    {
        Tile t = tiles.ContainsKey(pos) ? tiles[pos] : null;
        marker.localPosition = t != null ? t.center : new Vector3(pos.x, 0, pos.y);
    }

    public void MoveTileSelectionUpwards()
    {
        pos += new Point(0, 1);
    }

    public void MoveTileSelectionDownwards()
    {
        pos -= new Point(0, 1);
    }

    public void MoveTileSelectionLeft()
    {
        pos -= new Point(1, 0);
    }
    public void MoveTileSelectionRight()
    {
        pos += new Point(1, 0);
    }

    public void MoveTileSelection(Vector2 mousePosition)
    {
        pos = new Point((int)mousePosition.x, (int)mousePosition.y);
    }

    #endregion

    #region LevelData
    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject);

        tiles.Clear();
        tilesScript.Clear();

        props.Clear();
        propData.Clear();
        //ClearEnemySpawnPoints();
        ClearPlayerSpawnPoints();

        rank = 0;
        missionName = null;
        missionType = Type.Hunt;
        zone = Zone.Desert;
        hazard = Hazard.Thunderstorm;

        otherCreatures = null;

        money = 0;

        items = null;

    }

    public void Save()
    {
        string filePath = Application.dataPath + "/Resources/Levels";
        if (!Directory.Exists(filePath))
            CreateSaveDirectory();

        LevelData board = ScriptableObject.CreateInstance<LevelData>();

        board.tiles = new List<Vector3>(tilesScript.Count);

        board.tileData = new List<DataTile>(tilesScript.Count);
        board.sprites = new List<Sprite>(tilesScript.Count);
        board.tileContent = new List<ObstacleType>(tilesScript.Count);
        board.playerSpawnPoints = new List<Point>(playerSpawnPoints.Count);
        board.enemySpawnPoints = new List<Point>(enemySpawnPoints.Count);
        board.propData = new List<PropData>(propData.Count);
        board.props = new List<Vector3>(props.Count);

        foreach (Tile t in tiles.Values)
        {
            board.sprites.Add(t.tileSprite.sprite);

            board.tiles.Add(new Vector3(t.pos.x, t.height, t.pos.y));

            if (t.content != null)
            {
                board.tileContent.Add(ObstacleType.RegularObstacle);
            }
            else
            {
                board.tileContent.Add(ObstacleType.Null);
            }
        }

        foreach (Point p in playerSpawnPoints)
        {
            board.playerSpawnPoints.Add(p);
        }

        foreach (Point p in enemySpawnPoints)
        {
            board.enemySpawnPoints.Add(p);
        }

        foreach (Tile t in tilesScript)
        {
            board.tileData.Add(t.data);
        }

        foreach(Prop p in props.Values)
        {
            board.props.Add(new Vector3(p.pos.x, p.height, p.pos.y));
        }

        foreach(PropData p in propData)
        {
            board.propData.Add(p);
        }

        board.rank = rank;
        board.missionName = missionName;
        board.type = missionType;

        board.zone = zone;
        board.hazard = hazard;
        board.otherCreatures = otherCreatures;
        board.money = money;
        board.items = items;


        string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, "Mission_" + rank + "_" + position);
        AssetDatabase.CreateAsset(board, fileName);
    }
    void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");
        filePath += "/Levels";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        AssetDatabase.Refresh();
    }

    public void Load()
    {
        Clear();

        if (levelData == null)
        {
            Debug.Log("No level data");
            return;
        }


        for (int i = 0; i < levelData.tiles.Count; i++)
        {
            Tile t = LoadTileBoard(i);
            t.Load(levelData.tiles.ToArray()[i]);


            tiles.Add(t.pos, t);
            tilesScript.Add(t);
        }

        for (int i = 0; i < levelData.props.Count; i++)
        {
            Prop p = LoadPropBoard(i);
            p.Load(levelData.props[i]);

            props.Add(p.pos, p);
            propData.Add(p.data);
        }
        foreach (Point p in levelData.playerSpawnPoints)
        {
            playerSpawnPoints.Add(p);
        }

        foreach (Point p in levelData.enemySpawnPoints)
        {
            enemySpawnPoints.Add(p);
        }

        rank = levelData.rank;
        missionName = levelData.missionName;
        missionType = levelData.type;
        zone = levelData.zone;
        hazard = levelData.hazard;

        if (levelData.otherCreatures != null)
        {
            otherCreatures = levelData.otherCreatures;
        }

        money = levelData.money;

        if (levelData.items != null)
        {
            items = levelData.items;
        }
    }

    #endregion

    #region UnitSpawn
    public void ClearPlayerSpawnPoints()
    {
        //foreach(Point p in playerSpawnPoints)
        //{
        //    if(playerSpawnDictionary[p].gameObject != null)
        //    {
        //        DestroyImmediate(playerSpawnDictionary[p].gameObject);
        //    }
        //}

        playerSpawnDictionary.Clear();
        playerSpawnPoints.Clear();
    }
    public void AddPlayerSpawnPoint()
    {
        AddSpawnPoint(playerSpawnPoints, howManyPlayers);

        GameObject instance = Instantiate(playerSpawnPrefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
        instance.transform.parent = transform;
        playerSpawnDictionary.Add(pos, instance);
    }

    public void ClearEnemySpawnPoints()
    {
        //foreach (Point p in enemySpawnPoints)
        //{
        //    DestroyImmediate(enemySpawnDictionary[p].gameObject);
        //}
        enemySpawnDictionary.Clear();
        enemySpawnPoints.Clear();
    }
    public void AddEnemySpawnPoint()
    {
        AddSpawnPoint(enemySpawnPoints, howManyEnemies);

        GameObject instance = Instantiate(enemySpawnPrefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
        instance.transform.parent = transform;
        enemySpawnDictionary.Add(pos, instance);
    }
    public void AddSpawnPoint(List<Point> list, int limit)
    {
        if (list.Count < limit && !list.Contains(pos))
        {
            list.Add(pos);
        }

        else
        {
            Debug.Log("Warning, there aren't anymore spawn points left");
        }
    }
    #endregion

    

}
