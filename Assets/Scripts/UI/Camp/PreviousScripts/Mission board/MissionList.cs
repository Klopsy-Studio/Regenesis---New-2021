using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Mission/New Mission list")]
public class MissionList : ScriptableObject
{
     public List<LevelData> missions;
}