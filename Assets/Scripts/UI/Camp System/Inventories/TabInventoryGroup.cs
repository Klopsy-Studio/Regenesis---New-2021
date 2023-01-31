using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabInventoryGroup : MonoBehaviour
{
    public List<TabInventoryButton> tabInventoryButtons;
    //public Sprite tabIdle;
    //public Sprite tabHover;
    //public Sprite tabActive;

    public TabInventoryButton selectedTab;
    public List<GameObject> objectsToSwap;
    public void Subscribe(TabInventoryButton button)
    {
        if(tabInventoryButtons == null)
        {
            tabInventoryButtons = new List<TabInventoryButton>();
        }

        tabInventoryButtons.Add(button);
    }

    public void OnTabEnter(TabInventoryButton button)
    {

        ResetTabs();
        //if(selectedTab == null || button != selectedTab)
        //{
        //    button.background.sprite = tabHover;
        //}
     
    }

    public void OnTabExit(TabInventoryButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabInventoryButton button)
    {
        selectedTab = button;
        ResetTabs();
        //button.background.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        //foreach (TabInventoryButton button in tabInventoryButtons)
        //{
        //    if(selectedTab!= null && button == selectedTab) { continue; }
        //    button.background.sprite = tabIdle;
        //}
    }
}
