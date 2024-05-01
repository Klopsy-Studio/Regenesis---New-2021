using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCampModes : MonoBehaviour, IDataPersistence
{
    [SerializeField] List<OpenNewTab> campButtons;
    [SerializeField] List<Button> campButtonsComponent;
    [SerializeField] CanvasScaler campCanvas;
    [SerializeField] float windowScaleFactor;
    GameObject currentStructure;
    public List<GameObject> structures;

    public bool isTutorialFinished;
    [SerializeField] GameObject tutorial;
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
        if(campButtonsComponent != null)
        {
            foreach (Button o in campButtonsComponent)
            {
                o.interactable = true;
            }
        }
        //if(campButtons != null)
        //{
        //    if(campButtons.Count > 0)
        //    {
        //        foreach (OpenNewTab o in campButtons)
        //        {
        //            o.gameObject.GetComponent<Button>().interactable = true;
        //        }
        //    }
        //}
    }

    public void DisableAllButtons()
    {
        if (campButtonsComponent != null)
        {
            foreach (Button o in campButtonsComponent)
            {
                o.interactable = false;
            }
        }
    }



    private void Start()
    {
        if (!isTutorialFinished)
        {
            isTutorialFinished = true;
            tutorial.SetActive(true);
        }
    }

    public void LoadData(GameData data)
    {
        isTutorialFinished = data.isMainCampTutorialFinished;
    }

    public void SaveData(GameData data)
    {
        data.isMainCampTutorialFinished = isTutorialFinished;
    }
}
