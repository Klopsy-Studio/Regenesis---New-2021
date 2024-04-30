using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AcceptMissionButton : MonoBehaviour, IPointerClickHandler
{

    [HideInInspector] public LevelData mission;
    [HideInInspector] public MissionInfoPanel missionInfoPanel;
    [SerializeField] MoveToAnotherScene sceneManager;
    [SerializeField] Animator sceneTransition;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(mission != null)
        {
            DataPersistenceManager.instance.SaveGame();
            GameManager.instance.currentMission = mission;
            sceneManager.ChangeSceneToLoad("Battle");
            sceneManager.GoToNewScene();
            missionInfoPanel.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Accept Mission is null");
        }
       
    }

    public void LoadSceneAfterAnimation()
    {

    }
}
