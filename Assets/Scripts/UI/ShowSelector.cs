using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ShowSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject selector;
    public void OnPointerEnter(PointerEventData eventData)
    {
        selector.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selector.gameObject.SetActive(false);
    }
}
