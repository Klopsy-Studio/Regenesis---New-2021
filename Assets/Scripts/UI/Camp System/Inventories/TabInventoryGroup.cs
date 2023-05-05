using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabInventoryGroup : MonoBehaviour
{

    [Header("tutorialVariable")]
    [SerializeField] GameObject tutorialPanel;
    bool isTutorialFinished = false;


    public Color defaultColor;
    public Color selectedColor;
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

    //public void OnTabEnter(TabInventoryButton button)
    //{

    //    ResetTabs();
    //    //if(selectedTab == null || button != selectedTab)
    //    //{
    //    //    button.background.sprite = tabHover;
    //    //}
     
    //}

    //public void OnTabExit(TabInventoryButton button)
    //{
    //    ResetTabs();
    //}

    public void OnTabSelected(TabInventoryButton button)
    {
        selectedTab = button;
        ResetTabs();
        //button.background.sprite = tabActive;
        selectedTab.tabImage.color = selectedColor;
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
        foreach (TabInventoryButton button in tabInventoryButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            button.tabImage.color = defaultColor;
        }
    }

    private void Start()
    {
        tutorialPanel.SetActive(false);
        if (!isTutorialFinished) { tutorialPanel.SetActive(true); }
    }
    public void FinishTutorial() //UnityButtons
    {
        tutorialPanel.SetActive(false);
        isTutorialFinished = true;
    }

}
