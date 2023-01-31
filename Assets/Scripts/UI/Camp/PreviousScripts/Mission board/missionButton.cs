using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionButton : MonoBehaviour
{
    [Header("Mission")]
    [SerializeField] public LevelData missionData;

    [Header("UI objects")]
    [SerializeField] public Text textName;
    [HideInInspector] public MissionBoard board;

    [Header("UI variables")]
    public bool selected = false;
    public void LoadMission()
    {
        if (selected)
        {
            board.LoadMission();
        }

        else
        {
            board.currentButton.selected = false;
            board.currentButton = this;
            board._currentMission = missionData;
            board.UpdateDetails();
            selected = true;
        }

    }
}
