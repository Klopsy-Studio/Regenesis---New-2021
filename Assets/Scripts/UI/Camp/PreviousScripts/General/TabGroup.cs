using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour //THIS SCRIPT HAS BEEN MODIFIED. IT DOES NOT HAVE ITS ORIGINAL PURPOSE
{
    [SerializeField] public List<TabButtonUI> tabButtons;
    [SerializeField] public TabButtonUI selectedTabButton;
    
    [SerializeField] public List<GameObject> objectsToSwap;

    public void Subscribe(TabButtonUI button)
    {
        if (tabButtons == null)
            tabButtons = new List<TabButtonUI>();

        tabButtons.Add(button);
    }

    //public void OnTabEnter(TabButtonUI button)
    //{
    //    ResetTabsToIdle();
    //    if (selectedTabButton == null || button != selectedTabButton)
    //        button.currentImage.sprite = tabHover;

    //}
    public void OnTabExit(TabButtonUI button)
    {
        ResetTabsToIdle();
    }
    public void OnTabSelected(TabButtonUI button)
    {
        //THIS SCRIPT HAS BEEN MODIFIED. IT DOES NOT HAVE ITS ORIGINAL PURPOSE

        selectedTabButton = button;
        ResetTabsToIdle();
        button.currentImage.sprite = button.selectedImage;
        int index = button.transform.GetSiblingIndex();


        if (index == 0)
        {
            for (int i = 0; i < objectsToSwap.Count; i++)
            {
                objectsToSwap[i].SetActive(false);

            }
            objectsToSwap[0].SetActive(true);
        }
        else if (index == 1)
        {
            for (int i = 0; i < objectsToSwap.Count; i++)
            {
                objectsToSwap[i].SetActive(false);

            }
            objectsToSwap[1].SetActive(true);
        }
        else if (index > 1)
        {
            for (int i = 0; i < objectsToSwap.Count; i++)
            {
                objectsToSwap[i].SetActive(false);

            }
            objectsToSwap[2].SetActive(true);
        }


        //for (int i = 0; i < objectsToSwap.Count; i++)
        //{

        //    if (i == index)
        //        objectsToSwap[i].SetActive(true);
        //    else
        //        objectsToSwap[i].SetActive(false);
        //}
    }

    public void ResetTabsToIdle()
    {
        foreach (TabButtonUI button in tabButtons)
        {
            if (selectedTabButton != null && button == selectedTabButton)
                continue;
            button.currentImage.sprite = button.idleImage;
        }
    }
}