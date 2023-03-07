using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTipHandlerHover : MonoBehaviour
{
    [SerializeField] private List<TooltipInfos> tooltipContentList;
    [SerializeField] private GameObject tooltipContainer;
    [SerializeField] private TMP_Text _tooltipDescriptionTMP;

    private void OnEnable()
    {
        LinkHandlerForTMPTextOnHover.OnHoverOnLinkEvent += GetToolTipInfo;
        LinkHandlerForTMPTextOnHover.OnCloseTooltipEvent += CloseToolTip;
    }

    private void OnDisable()
    {
        LinkHandlerForTMPTextOnHover.OnHoverOnLinkEvent -= GetToolTipInfo;
        LinkHandlerForTMPTextOnHover.OnCloseTooltipEvent -= CloseToolTip;
    }

    private void GetToolTipInfo(string keyword, Vector3 mousePos)
    {
       
        foreach (var entry in tooltipContentList)
        {
            if (entry.keyword == keyword)
            {
                if (!tooltipContainer.gameObject.activeInHierarchy)
                {
                    Debug.Log("ha entrado");
                    //tooltipContainer.transform.position = mousePos + new Vector3(0, 100, 0);
                    tooltipContainer.SetActive(true);
                }

                _tooltipDescriptionTMP.text = entry.Description;
             
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
