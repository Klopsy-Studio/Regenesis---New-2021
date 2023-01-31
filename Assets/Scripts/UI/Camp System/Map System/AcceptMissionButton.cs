using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AcceptMissionButton : MonoBehaviour, IPointerClickHandler
{

    [HideInInspector] public LevelData mission;
    [HideInInspector] public MissionInfoPanel missionInfoPanel;

    [SerializeField] Animator sceneTransition;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(mission != null)
        {
            GameManager.instance.currentMission = mission;
            missionInfoPanel.gameObject.SetActive(false);
            GameManager.instance.sceneToLoad = "Battle";
            sceneTransition.SetTrigger("fadeIn");
            Invoke("LoadSceneAfterAnimation", 2f);
        }
        else
        {
            Debug.Log("Accept Mission is null");
        }
       
    }

    public void LoadSceneAfterAnimation()
    {
        SceneManager.LoadScene("LoadingScreen");
    }
}
