using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionDetails : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Text missionNameText;
    [SerializeField] Text zoneText;
    [SerializeField] Text hazardText;
    [SerializeField] Text otherCreaturesText;
    [SerializeField] Text moneyText;
    [SerializeField] Text itemsText;

    [SerializeField] Image missionImage;


    public void UpdateDetails(LevelData data)
    {
        missionNameText.text = data.missionName;

        switch (data.zone)
        {
            case Zone.Desert:
                zoneText.text = "Dessert";
                break;
            default:
                break;
        }

        switch (data.hazard)
        {
            case Hazard.Thunderstorm:
                hazardText.text = "Thunderstorm";
                break;

            case Hazard.Sandstorm:
                hazardText.text = "Sandstorm";
                break;

            default:
                break;
        }

        if(data.otherCreatures != null)
        {
            foreach (string line in data.otherCreatures)
            {
                otherCreaturesText.text += "\n-" + line;
            }
        }
        else
        {
            Debug.Log("Mission has no other creatures");
        }
        

        moneyText.text = data.money.ToString();

        if(data.items != null)
        {
            foreach (string line in data.items)
            {
                itemsText.text += "\n-" + line;
            }
        }
        else
        {
            Debug.Log("Mission gives no items");
        }
        if(data.monsterImage != null)
        {
            missionImage.sprite = data.monsterImage;
        }
        else
        {
            Debug.Log("There is no monster image");
        }
    }


}
