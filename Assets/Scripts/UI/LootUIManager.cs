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


    public IEnumerator LootSequence(DroppedMaterials monsterMaterialList)
    {
        //Waiting for the frame to come down
        yield return new WaitForSeconds(1f);

        //Setting the variables
        huntName.SetText(controller.levelData.missionLocation);
        objectiveName.SetText(controller.enemyUnits[0].unitName);
        int time = (int)controller.huntTime;
        int minutesTime = time / 60;
        int secondsTime = time % 60;

        huntTime.SetText(minutesTime.ToString() + " min. " + secondsTime + " sec. ");


        huntName.gameObject.SetActive(true);
        AudioManager.instance.Play("Boton4");
        yield return new WaitForSeconds(0.5f);

        objectiveName.gameObject.SetActive(true);
        AudioManager.instance.Play("Boton4");

        yield return new WaitForSeconds(0.5f);

        huntTime.gameObject.SetActive(true);
        AudioManager.instance.Play("Boton4");

        yield return new WaitForSeconds(0.5f);


        for (int i = 0; i < monsterMaterialList.materialContainer.Count; i++)
        {
            LootDrop obj = Instantiate(lootDropSlot, Vector3.zero, Quaternion.identity, parentTransform).GetComponent<LootDrop>();
            obj.materialIcon.sprite = monsterMaterialList.materialContainer[i].material.sprite;
            obj.ammountText.SetText(monsterMaterialList.materialContainer[i].amount.ToString());
            AudioManager.instance.Play("Boton4");

            yield return new WaitForSeconds(0.2f);
        }


        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.Play("Boton4");

        //Set the monster image

        monsterImage.sprite = controller.levelData.monsterPicture;
        monsterImage.gameObject.SetActive(true);




        AudioManager.instance.Play("Boton4");

        returnToCampButton.gameObject.SetActive(true);
    }
    public IEnumerator LootDefeat()
    {
        //Waiting for the frame to come down
        yield return new WaitForSeconds(1f);

        //Setting the variables
        huntName.SetText(controller.levelData.missionLocation);
        objectiveName.SetText(controller.enemyUnits[0].unitName);
        int time = (int)controller.huntTime;
        int minutesTime = time / 60;
        int secondsTime = time % 60;

        huntTime.SetText(minutesTime.ToString() + " ' " + secondsTime + "\"");


        huntName.gameObject.SetActive(true);
        AudioManager.instance.Play("Boton4");
        yield return new WaitForSeconds(0.5f);

        objectiveName.gameObject.SetActive(true);
        AudioManager.instance.Play("Boton4");

        yield return new WaitForSeconds(0.5f);

        huntTime.gameObject.SetActive(true);
        AudioManager.instance.Play("Boton4");

        yield return new WaitForSeconds(0.5f);

        AudioManager.instance.Play("Boton4");

        //Set the monster image

        AudioManager.instance.Play("Boton4");

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
