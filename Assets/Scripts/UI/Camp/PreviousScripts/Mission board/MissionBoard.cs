using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionBoard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject uIMissionList;
    [SerializeField] private GameObject uIMissionObject;
    [SerializeField] private MissionDetails uiDetails;

    [Header("Missions")]
    [SerializeField] private List<LevelData> _allMissions;

    [SerializeField] private MissionList _missionList;
    public LevelData _currentMission;
    public MissionButton currentButton;

    private void Start()
    {
        GatherAllMissions();
        DisplayMissions(1);
    }

    private void GatherAllMissions()
    {

        for (int i = 0; i < _missionList.missions.Count; i++)
        {
            if (_missionList.missions[i] != null)
            {
                _missionList.missions[i].GenerateId();
                _allMissions.Add(_missionList.missions[i]);
            }
        }
    }

    private void DisplayMissions(int rank)
    {
        for (int i = 0; i < _allMissions.Count; i++)
        {
            if (_allMissions[i].rank == rank)
            {
                Debug.Log("Mission generated");
                GameObject mission = Instantiate(uIMissionObject, uIMissionList.transform);

                //MissionButton currentMission = mission.GetComponent<MissionButton>().name.text = _allMissions[i].missionName;
                MissionButton currentMission = mission.GetComponent<MissionButton>();

                currentMission.textName.text = _allMissions[i].missionName;
                currentMission.missionData = _allMissions.ToArray()[i];
                currentMission.board = this;

                if(currentButton == null)
                {
                    currentButton = currentMission;
                }
                if(_currentMission == null)
                {
                    _currentMission = currentMission.missionData;
                    currentMission.selected = true;
                    UpdateDetails();
                }
            }
        }
    }

    public void ShowMission(string id)
    {
        for (int i = 0; i < _allMissions.Count; i++)
        {
            if (id == _allMissions[i].id)
            {
                _currentMission = _allMissions[i];
            }
        }
    }

    public void LoadMission()
    {
        GameManager.instance.LoadMission();
    }

    public void UpdateDetails()
    {
        uiDetails.UpdateDetails(_currentMission);
    }
}