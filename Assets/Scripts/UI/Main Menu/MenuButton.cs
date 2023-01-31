using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Button Checks")]
    public bool canBeSelected;
    public bool selected;
    [Space]

    [Header("Variables")]
    [SerializeField] Color defaultTextColor;
    [SerializeField] Color highlightTextColor;
    [Space]
    [Header("References")]
    [SerializeField] Text buttonText;

    public UnityEvent action;
    public UnityEvent onHover;
    public UnityEvent onExit;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(canBeSelected && selected)
        {
            if(action != null)
            {
                action.Invoke();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canBeSelected)
        {
            onHover.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canBeSelected)
        {
            onExit.Invoke();
        }
    }

    public void HighlightText()
    {
        buttonText.color = highlightTextColor;
    }

    public void DefaultText()
    {
        buttonText.color = defaultTextColor;
    }

    public void Selected()
    {
        selected = true;
    }

    public void UnSelected()
    {
        selected = false;
    }
}
