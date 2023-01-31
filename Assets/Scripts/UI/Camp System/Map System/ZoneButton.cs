using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZoneButton : MonoBehaviour, IPointerClickHandler
{
    public int mapID;
    MapManager mapManager;
    [SerializeField] Image notificationIMG;
    public void FillVariables(MapManager _mapManager)
    {
        mapManager = _mapManager;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
       
        mapManager.OpenMapPanelList(mapID);
        mapManager.missionInfoPanel.gameObject.SetActive(false);
    }

    public void UpdateNotifications()
    {
        foreach (var missions in mapManager.displayMapContainerList[mapID].missionList.missions)
        {
            var missionContainer = mapManager.allMissionsDictionary[missions];
            if (missionContainer.isNew)
            {
                notificationIMG.gameObject.SetActive(true);
            }
            else
            {
                notificationIMG.gameObject.SetActive(false);
            }
        }   
    }
}
