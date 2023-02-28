using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCampModes : MonoBehaviour
{
    [SerializeField] List<OpenNewTab> campButtons;
    [SerializeField] CanvasScaler campCanvas;
    [SerializeField] float windowScaleFactor;
    GameObject currentStructure;
    public List<GameObject> structures;
    // Update is called once per frame
    public void SetNormalScale()
    {
        currentStructure.SetActive(false);
        currentStructure = null;
        campCanvas.referenceResolution = new Vector2(1920f, 1080f);
    }

    public void SetStructure(GameObject structureToOpen)
    {
        if(currentStructure != null)
        {
            if(currentStructure != structureToOpen)
            {
                currentStructure.SetActive(false);
            }
        }
        structureToOpen.SetActive(true);
        currentStructure = structureToOpen;
        campCanvas.referenceResolution = new Vector2(windowScaleFactor, 1080f);
    }

    public void EnableAllButtons()
    {
        if(campButtons != null)
        {
            if(campButtons.Count > 0)
            {
                foreach (OpenNewTab o in campButtons)
                {
                    o.EnableButton();
                }
            }
            else
            {
                Debug.Log("Camp buttons is count = 0");
            }
        }
        else
        {
            Debug.Log("Camp buttons is null");
        }
    }

    public void DisableAllButtons()
    {
        if (campButtons != null)
        {
            if (campButtons.Count > 0)
            {
                foreach (OpenNewTab o in campButtons)
                {
                    o.DisableButton();
                }
            }
            else
            {
                Debug.Log("Camp buttons is count = 0");

            }
        }

        else
        {
            Debug.Log("Camp buttons is null");
        }
        
    }
}
