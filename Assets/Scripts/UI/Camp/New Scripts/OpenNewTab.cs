using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenNewTab : MonoBehaviour, IPointerClickHandler
{
    public bool buttonEnabled; 
    [SerializeField] CanvasScaler canva;
    Vector2 smallWindow = new Vector2(640, 360);
    [SerializeField] GameObject tabToOpen;
    //[SerializeField] GameObject gameObjectAnimator;


    [Header("Close buttons ")]
    [SerializeField] GameObject[] closeButtons;

    private void Start()
    {
        EnableButton();
    }
    public void OpenTab()
    {
        if (buttonEnabled)
        {
            if (tabToOpen != null)
            {
                Debug.Log("GOLPEA");
                //canva.referenceResolution = smallWindow;
                tabToOpen.SetActive(true);
            }
            else
            {
                Debug.Log("There's no tab to open");
            }
        }
        
    }

  
    
   

    public void EnableButton()
    {
        buttonEnabled = true;
    }

    public void DisableButton()
    {
        buttonEnabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("hola");
        if (eventData.button !=PointerEventData.InputButton.Left)
        {
            return;
        }
        Debug.Log("funciona");
        OpenTab();
        foreach (var item in closeButtons)
        {
            item.SetActive(false);
        }

    }
}
