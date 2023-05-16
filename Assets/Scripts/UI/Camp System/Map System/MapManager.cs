using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour, IDataPersistence
{
    


    //[SerializeField] ZoneButton[] zoneButtons;
    [SerializeField] GameObject[] mapLists;
    public DisplayMapContainers[] displayMapContainerList;
    public MissionInfoPanel missionInfoPanel;
    public AcceptMissionButton acceptMissionButton;

    public Dictionary<LevelData, MissionContainer> allMissionsDictionary = new Dictionary<LevelData, MissionContainer>();
    public List<MissionContainer> allMisionsList = new List<MissionContainer >();

    [Header("tutorialVariable")]
    bool isTutorialFinished = false;
    [SerializeField] GameObject tutorialPanel;
    private void Start()
    {
        tutorialPanel.SetActive(false);
        if (!isTutorialFinished) { tutorialPanel.SetActive(true); }

        missionInfoPanel.gameObject.SetActive(false);


        //foreach (var button in zoneButtons)
        //{
        //    button.FillVariables(this);
        //}


        SetUpMissions();
        //foreach (var button in zoneButtons)
        //{
        //    button.UpdateNotifications();
        //}
    }

    public void FinishTutorial() //UnityButtons
    {
        tutorialPanel.SetActive(false);
        isTutorialFinished = true;
    }

    //private void Update()
    //{
    //    UnlockMissions();
    //}

    private void OnEnable()
    {
        UnlockMissions();
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
                    Debug.Log("aaaaa");
                    MissionContainer missionContainer = allMissionsDictionary[unlockableMissions[i]];
                    missionContainer.gameObject.SetActive(false);
                   
                }
            }
        }
        //foreach (var displayMapContainer in displayMapContainerList)
    }

    void UnlockMissions()
    {
        foreach (var displayMapContainerList in displayMapContainerList)
        {
            displayMapContainerList.UpdateMissionContainers();
        }
        foreach (var mission in allMisionsList)
        {
            Debug.Log("a");
            if (mission.levelData.UnlockableMissions != null && mission.levelData.hasBeenCompleted)
            {
                Debug.Log("B");
                var unlockableMissions = mission.levelData.UnlockableMissions;
                for (int i = 0; i < unlockableMissions.Length; i++)
                {
                    MissionContainer missionContainer = allMissionsDictionary[unlockableMissions[i]];
                    missionContainer.gameObject.SetActive(true);
                    Debug.Log("hoa");
                }
            }
            else
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
    }

   

    public void UpdateMapManager()
    {
        foreach (var mission in allMisionsList)
        {
            if (mission.levelData.isOld)
            {
                mission.notificationIMG.gameObject.SetActive(false);
            }
            else
            {
                mission.notificationIMG.gameObject.SetActive(true);
                
            }
        }

        //foreach (var button in zoneButtons)
        //{
        //    button.UpdateNotifications();
        //}
    }

    public void LoadData(GameData data)
    {
        isTutorialFinished = data.isMapTutorialFinished;
    }

    public void SaveData(GameData data)
    {
        data.isMapTutorialFinished = isTutorialFinished;
    }
}
