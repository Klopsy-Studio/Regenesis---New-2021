using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopBackButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Button button;
    [SerializeField] bool activateButton;
    [SerializeField] ShopManager shopManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (activateButton)
        {
            button.onClick.Invoke();
        }
      
    }
}
