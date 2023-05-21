using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CampCursorUIInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool canSelect = true;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (canSelect)
        {
            GameCursor.instance.SetRegularCursor();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canSelect)
        {
            GameCursor.instance.SetHandCursor();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canSelect)
        {
            GameCursor.instance.SetRegularCursor();
        }
    }


    

    

 
}
