using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMapContainers : MonoBehaviour
{
    public MissionList missionList;
    [SerializeField] GameObject missionPrefab;
    [HideInInspector] public MapManager mapManager;

    public List<MissionContainer> missionContainerList;

    public void CreateMissionContainers()
    {
        for (int i = 0; i < missionList.missions.Count; i++)
        {
            var mission = missionList.missions[i];

            var obj = Instantiate(missionPrefab, Vector3.zero, Quaternion.identity, transform);
            var missionContainer = obj.GetComponent<MissionContainer>();
            missionContainer.levelData = mission;
            missionContainer.FillVariables(mapManager);

            if (missionContainer.levelData.hasBeenCompleted)
            {
              
                missionContainer.completedIMG.gameObject.SetActive(true);
                missionContainer.uncompletedIMG.gameObject.SetActive(false);
            }
            else
            {
                missionContainer.completedIMG.gameObject.SetActive(false);
                missionContainer.uncompletedIMG.gameObject.SetActive(true);
            }

            if(missionContainer.levelData.isOld)
            {
                missionContainer.notificationIMG.gameObject.SetActive(false);
            }
            else
            {
                missionContainer.notificationIMG.gameObject.SetActive(true);
              
            }
            missionContainerList.Add(missionContainer);
        }
    }

    public void UpdateMissionContainers()
    {
        foreach (var missionContainer in missionContainerList)
        {
            if (missionContainer.levelData.hasBeenCompleted)
            {

                missionContainer.completedIMG.gameObject.SetActive(true);
                missionContainer.uncompletedIMG.gameObject.SetActive(false);
            }
            else
            {
                missionContainer.completedIMG.gameObject.SetActive(false);
                missionContainer.uncompletedIMG.gameObject.SetActive(true);
            }

            if (missionContainer.levelData.isOld)
            {
                missionContainer.notificationIMG.gameObject.SetActive(false);
            }
            else
            {
                missionContainer.notificationIMG.gameObject.SetActive(true);

            }
        }
    }
}
