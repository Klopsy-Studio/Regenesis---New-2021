using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] ZoneButton[] zoneButtons;
    [SerializeField] GameObject[] mapLists;
    public DisplayMapContainers[] displayMapContainerList;
    public MissionInfoPanel missionInfoPanel;
    public AcceptMissionButton acceptMissionButton;

    public Dictionary<LevelData, MissionContainer> allMissionsDictionary = new Dictionary<LevelData, MissionContainer>();
    public List<MissionContainer> allMisionsList = new List<MissionContainer >();
   
    private void Start()
    {
        missionInfoPanel.gameObject.SetActive(false);

        //
        foreach (var button in zoneButtons)
        {
            button.FillVariables(this);
        }


        SetUpMissions();
        foreach (var button in zoneButtons)
        {
            button.UpdateNotifications();
        }
    }



    public void OpenMapPanelList(int mapSelectorId)
    {
        
        foreach (var map in mapLists)
        {
            map.SetActive(false);
        }

        mapLists[mapSelectorId].SetActive(true);
    }

    public void SetUpMissions()
    {
        foreach (var displayMapContainerList in displayMapContainerList)
        {
            displayMapContainerList.mapManager = this;
            displayMapContainerList.CreateMissionContainers();
        }

        foreach (var mission in allMisionsList)
        {
            if (mission.levelData.UnlockableMissions != null && !mission.levelData.hasBeenCompleted)
            {
                var unlockableMissions = mission.levelData.UnlockableMissions;
                for (int i = 0; i < unlockableMissions.Length; i++)
                {
                    MissionContainer missionContainer = allMissionsDictionary[unlockableMissions[i]];
                    missionContainer.gameObject.SetActive(false);
                    Debug.Log("hoa");
                }
            }
        }
        //foreach (var displayMapContainer in displayMapContainerList)
    }


   

    public void UpdateMapManager()
    {
        foreach (var mission in allMisionsList)
        {
            if (mission.isNew)
            {
                mission.notificationIMG.gameObject.SetActive(true);
            }
            else
            {
                mission.notificationIMG.gameObject.SetActive(false);
            }
        }

        foreach (var button in zoneButtons)
        {
            button.UpdateNotifications();
        }
    }


}
