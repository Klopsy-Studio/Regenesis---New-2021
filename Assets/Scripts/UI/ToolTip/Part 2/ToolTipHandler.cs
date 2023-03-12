using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipHandler : MonoBehaviour
{
    [SerializeField] private List<TooltipInfos> tooltipContentList; 

    [SerializeField] private GameObject tooltipContainer;
    [SerializeField] TMP_Text _tooltipTitleTMP;
    [SerializeField] TMP_Text _tooltipDescriptionTMP;
    [SerializeField] private Image iconDisplay;

 

    private void OnEnable()
    {
        LinkHandlerForTMPText.OnClickedOnLinkEvent += GetToolTipInfo;
    }

    private void OnDisable()
    {
        LinkHandlerForTMPText.OnClickedOnLinkEvent -= GetToolTipInfo;
    }

    void GetToolTipInfo(string keyword)
    {
        foreach (var entry in tooltipContentList)
        {
            if(entry.keyword == keyword)
            {
                if (!tooltipContainer.gameObject.activeInHierarchy)
                {
                    tooltipContainer.gameObject.SetActive(true);
                }

                _tooltipTitleTMP.text = entry.keyword;
                _tooltipDescriptionTMP.text = entry.Description;
                iconDisplay.sprite = entry.image;
                return;
            }
        }

        Debug.Log($"keyword: {keyword} not found");
    }

    public void CloseToolTip()
    {
        if (tooltipContainer.gameObject.activeInHierarchy)
        {
            tooltipContainer.SetActive(false);
        }
    }
}

[Serializable]
public struct TooltipInfos
{
    public string keyword;
    [TextArea]
    public string Description;
    public Sprite image;
}
