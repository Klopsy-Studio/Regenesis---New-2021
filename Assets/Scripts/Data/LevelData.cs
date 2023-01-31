using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type { Hunt }
public enum Zone { Desert }
public enum Hazard { Thunderstorm, Sandstorm };

[System.Serializable]
[CreateAssetMenu(menuName = "Mission/New Mission")]
public class LevelData : ScriptableObject
{
    [HideInInspector] public string id;

    [Header("General")]
    [SerializeField] private int position;
    [SerializeField] public int rank;
    [SerializeField] public string missionName;
    [SerializeField] public Type type;
    
    [Header("Image")]
    [SerializeField] public Sprite monsterImage;
    [SerializeField] private Sprite backgroundImage;

    [Header("Environment information")]
    [SerializeField] public Zone zone;
    [SerializeField] public Hazard hazard;
    [SerializeField] public List<string> otherCreatures;

    [Header("Rewards")]
    [SerializeField] public int money;
    [SerializeField] public List<string> items;

    //Units in game

    public GameObject enemyInLevel;
    public GameObject levelModel;
    //Variables to unlock new missions
    public bool hasBeenCompleted;
    public LevelData[] UnlockableMissions;

    //Mission Description
    //[TextArea]
    public string environmentDescription;
    public Sprite missionImage;


    public string GenerateId()
    {
        id = name.Substring(0, 3) + type.ToString().Substring(0,1) + rank.ToString() + position.ToString();
        return id;
    }

    #region Board parameters
     public List<Vector3> tiles;
     public List<DataTile> tileData;
    public List<Sprite> sprites;
     public List<Vector3> props;
     public List<PropData> propData;
     public List<ObstacleType> tileContent;
     public List<Point> playerSpawnPoints;
     public List<Point> enemySpawnPoints;
     public List<Point> itemSpawn;
    #endregion
}
