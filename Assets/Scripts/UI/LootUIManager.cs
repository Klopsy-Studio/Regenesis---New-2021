using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LootUIManager : MonoBehaviour
{
    [SerializeField] GameObject lootDropSlot;
    [SerializeField] Transform parentTransform;

    [SerializeField] BattleController controller;

    [SerializeField] TextMeshProUGUI huntName;
    [SerializeField] TextMeshProUGUI objectiveName;
    [SerializeField] TextMeshProUGUI huntTime;

    [SerializeField] Image monsterImage;

    [SerializeField] GameObject returnToCampButton;


    public IEnumerator LootSequence(DroppedMaterials dropped)
    {
        //Waiting for the frame to come down
        yield return new WaitForSeconds(1f);

        //Setting the variables
        huntName.SetText(controller.levelData.missionName);
        objectiveName.SetText(controller.enemyUnits[0].unitName);
        int time = (int)controller.huntTime;
        int minutesTime = time / 60;
        int secondsTime = time % 60;

        huntTime.SetText(minutesTime.ToString() + " min " + secondsTime + " ' ");


        huntName.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        objectiveName.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        huntTime.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);


        for (int i = 0; i < dropped.materialContainer.Count; i++)
        {
            LootDrop obj = Instantiate(lootDropSlot, Vector3.zero, Quaternion.identity, parentTransform).GetComponent<LootDrop>();
            obj.materialIcon.sprite = dropped.materialContainer[i].material.sprite;
            obj.ammountText.SetText(dropped.materialContainer[i].amount.ToString());

            yield return new WaitForSeconds(0.2f);
        }


        yield return new WaitForSeconds(0.5f);

        //Set the monster image

        monsterImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        returnToCampButton.gameObject.SetActive(true);
    }


    public void UpdateLootUI(/*DropsContainer dropContainer*/DroppedMaterials dropped)
    {
        for (int i = 0; i < dropped.materialContainer.Count; i++)
        {
            LootDrop obj = Instantiate(lootDropSlot, Vector3.zero, Quaternion.identity, parentTransform).GetComponent<LootDrop>();
            obj.materialIcon.sprite = dropped.materialContainer[i].material.sprite;
            obj.ammountText.SetText(dropped.materialContainer[i].amount.ToString());
        }

    }
}
