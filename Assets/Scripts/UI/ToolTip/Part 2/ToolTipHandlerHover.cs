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

<<<<<<< HEAD
    RectTransform tooltipRectTransform;



    private void Start()
    {
        tooltipRectTransform = tooltipContainer.GetComponent<RectTransform>();
    }

=======
  
   
>>>>>>> task/newTutorial
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

<<<<<<< HEAD
    private void Update()
    {

        Vector2 mousePosition = Input.mousePosition;
        float pivotX = mousePosition.x / Screen.width;
        float pivotY = mousePosition.y / Screen.height;

        float finalPivotX = 0f;
        float finalPivotY = 0f;

        if (pivotX < 0.5) //If mouse on left of screen move tooltip to right of cursor and vice vera
        {
            finalPivotX = -0.1f;
        }

        else
        {
            finalPivotX = 1.01f;
        }



        if (pivotY < 0.5) //If mouse on lower half of screen move tooltip above cursor and vice versa
        {
            finalPivotY = 0;
        }

        else
        {
            finalPivotY = 1;
        }


        tooltipRectTransform.pivot = new Vector2(finalPivotX, finalPivotY);


        tooltipContainer.transform.position = mousePosition;
    }
=======
  
>>>>>>> task/newTutorial
}
