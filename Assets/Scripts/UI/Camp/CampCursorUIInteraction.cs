using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CampCursorUIInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameCursor.instance.SetHandCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameCursor.instance.SetRegularCursor();
    }

 
}
