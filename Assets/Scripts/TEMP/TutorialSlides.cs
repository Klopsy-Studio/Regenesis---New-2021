using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSlides : MonoBehaviour
{
    int index = 0;

    [SerializeField] Sprite[] slides;
    [SerializeField] Image imageComponent;

    public void NextSlide()
    {
        if (index >= slides.Length-1)
        {
            index = 0;
        }
        else
        {
            index++;
        }

        imageComponent.sprite = slides[index];
    }

    public void PrevSlide()
    {
        if(index == 0)
        {
            index = slides.Length - 1;
        }
        else
        {
            index--;
        }
        imageComponent.sprite = slides[index];
    }

    public void ResetSlides()
    {
        index = 0;
        imageComponent.sprite = slides[index];
    }
}
