using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public GameObject[] slidesArray;
    public static event EventHandler<InfoEventArgs<int>> buttonClick;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ClickConfirm(int index)
    {
        if (buttonClick != null)
            buttonClick(this, new InfoEventArgs<int>(index));
    }

}
