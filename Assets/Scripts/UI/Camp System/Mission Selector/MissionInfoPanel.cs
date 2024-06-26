using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionInfoPanel : MonoBehaviour
{
   
    public Text missionName;
    public Text environmentDescription;
    public Image missionImage;
   



    public void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }

    public void UpdateMissionInfoPanel(LevelData _leveldata)
    {    
        missionName.text = _leveldata.missionName;
        environmentDescription.text = _leveldata.environmentDescription;

        if (_leveldata.hasBeenCompleted)
        {
            missionImage.sprite = _leveldata.missionImage;
        }
        else
        {
            missionImage.sprite = _leveldata.missionImageNotCompleted;
        }
        //zone.text = _environment;   
    }
}
