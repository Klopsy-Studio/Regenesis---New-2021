using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MonsterDropsContainer")]
public class DropsContainer : ScriptableObject
{
    public List<MonsterDrops> containerList = new List<MonsterDrops> ();
    public int minimumDrops;
}

[System.Serializable]
public class MonsterDrops
{
    public MonsterMaterial monsterMaterial;
    [Range(1, 100)]
    public int dropProbabilty;

}
