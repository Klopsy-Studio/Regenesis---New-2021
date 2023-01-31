using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMissionPlaceholder : MonoBehaviour
{
    [SerializeField] GameObject leaveForMissionButton;    

    public void ChooseMission(LevelData selectedMission)
    {
        GameManager.instance.currentMission = selectedMission;
        leaveForMissionButton.SetActive(true);
    }
}
