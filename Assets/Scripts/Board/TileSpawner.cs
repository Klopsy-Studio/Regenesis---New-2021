using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    Placeholder, Desert, NonPlayable, City
};
public enum DesertType
{
    Desert1, Desert2, Desert3, Quicksand
}

public enum PropType
{
    City, Desert, Park
}
public enum ThingToSpawn
{
    Tiles, Props
}


[System.Serializable]
[CreateAssetMenu(menuName = "Tiles/New Tile Spawner")]
public class TileSpawner : ScriptableObject

{
    //public typeOfTiles TypeToSpawn;
    [Header("Tiles")]
    //Desert
    public Sprite[] desertTiles;
    //Placeholder
    public Sprite[] placeholderTiles;

    public Sprite[] test;

    public TileSpriteData[] cityTiles;
    //Non Playable
    public Sprite[] nonPlayableTiles;

    [Space]
    [Header("Props")]
    public GameObject[] cityProps;

    public GameObject[] desertProps;

    public GameObject[] parkProps;
}

[System.Serializable]
public class TileSpriteData
{
    public string name;
    public TileType type;   
    public SpriteTile[] sprites;
}

public enum TileClass
{
    Corner, InnerCorner, Sides, Fillers, Misc
}

[System.Serializable]
public class SpriteTile
{
    public TileClass type;
    public Sprite[] Sprites;
}
