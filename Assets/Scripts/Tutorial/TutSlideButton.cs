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
     
        if(buttonToNextSlide != null)
        {
           
            buttonToNextSlide.onClick.AddListener(() =>
            {
               
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
