using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutSlideButton : MonoBehaviour
{
    [SerializeField] Button buttonToNextSlide;
    [SerializeField] Button buttonToNextState;

    private void Start()
    {
        Debug.Log("TUT SLIDE BUTTON INICIALIZA");
        if(buttonToNextSlide != null)
        {
            Debug.Log("TUT SLIDE BUTTON AAAAAAAAA");
            buttonToNextSlide.onClick.AddListener(() =>
            {
                Debug.Log("TUT SLIDE BUTTON bbbbbb");
                TutorialManager.instance.ClickConfirm(0);
            });
        
        }

        if(buttonToNextState != null)
        {
            buttonToNextState.onClick.AddListener(() =>
            {
                TutorialManager.instance.ClickConfirm(1);
            });
        }
    }
}
