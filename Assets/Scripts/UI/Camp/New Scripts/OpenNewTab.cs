using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenNewTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] CanvasScaler canva;
    Vector2 smallWindow = new Vector2(640, 360);
    [SerializeField] GameObject tabToOpen;
    //[SerializeField] GameObject gameObjectAnimator;

    public void OpenTab()
    {
     
        if(tabToOpen != null)
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

    public void OnPointerEnter(PointerEventData eventData)
    {
      
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

 
}
