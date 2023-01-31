using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public static event EventHandler<InfoEventArgs<int>> buttonClick;
    public static event EventHandler<InfoEventArgs<int>> buttonCancel;
    public BattleController battleController;

    [SerializeField] RectTransform selectorAction;
    [SerializeField] RectTransform selectorAbility;
    [SerializeField] RectTransform selectorItem;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void ClickConfirm(int index)
    {
        if (buttonClick != null)
            buttonClick(this, new InfoEventArgs<int>(index));
    }


    public void ClickCancel(int index)
    {
        if (buttonCancel != null)
            buttonCancel(this, new InfoEventArgs<int>(index));
    }

    public RectTransform ReturnSelector()
    {
        if(battleController.moveActionSelector)
        {
            //Debug.Log("SELECTOR ACTION");
            return selectorAction;
        }

        if (battleController.moveAbilitySelector)
        {
            //Debug.Log("SELECTOR ABILITY");
            return selectorAbility;
        }

        if (battleController.moveItemSelector)
        {
            //Debug.Log("SELECTOR ITEM");
            return selectorItem;
        }


        return null;

    }
}
