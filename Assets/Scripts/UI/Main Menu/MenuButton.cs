using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Button Checks")]
    public bool test;
    public bool canBeSelected;
    public bool selected;
    [Space]

    [Header("Variables")]
    [SerializeField] Color defaultTextColor;
    [SerializeField] Color highlightTextColor;
    [Space]
    [Header("References")]
    [SerializeField] Text buttonText;
    [SerializeField] Image buttonImage;

    public UnityEvent action;
    public UnityEvent onHover;
    public UnityEvent onExit;
    public UnityEvent onUp;


    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (canBeSelected && selected)
        {
            if (action != null)
            {
               action.Invoke();
            }
        }
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (canBeSelected && selected)
        {
            if(onUp != null)
            {
                onUp.Invoke();
            }
        }
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //if (canBeSelected && selected)
        //{
        //    if (action != null)
        //    {
        //        action.Invoke();
        //    }
        //}
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (canBeSelected)
        {
            onHover.Invoke();
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (canBeSelected)
        {
            onExit.Invoke();
        }
    }

    public virtual void HighlightText()
    {
        buttonText.color = highlightTextColor;
    }

    public virtual void DefaultText()
    {
        buttonText.color = defaultTextColor;
    }

    public virtual void ChangeButtonImage(Sprite newSprite)
    {
        buttonImage.sprite = newSprite;
    }
    public void Selected()
    {
        selected = true;
    }

    public void UnSelected()
    {
        selected = false;
    }

    public void ActivateButton()
    {
        canBeSelected = true;
    }

    public void DeactivateButton()
    {
        canBeSelected = false;
    }

    public void Log(string message)
    {
        Debug.Log(message);
    }
}
