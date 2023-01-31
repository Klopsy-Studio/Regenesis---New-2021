using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTab : MonoBehaviour
{
    [SerializeField] GameObject tab;
    [SerializeField] GameObject currentTab;

    public void OpenTab()
    {
        if(tab!= null)
        {
            tab.gameObject.SetActive(true);
        }

        if(currentTab != null)
        {
            currentTab.SetActive(false);
        }
    }
}
