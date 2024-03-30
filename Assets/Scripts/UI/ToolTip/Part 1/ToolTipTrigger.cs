using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    [Multiline]
    public string content;

    public bool changeToFinger;

    public bool allowTooltip = true;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!allowTooltip)
            return;

        ToolTipSystem.Show(content, header);
        ToolTipSystem.instance.currentTrigger = this;

        if (changeToFinger)
        {
            GameCursor.instance.SetHandCursor(); 
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!allowTooltip)
            return;
        ToolTipSystem.Hide();
        GameCursor.instance.SetRegularCursor();

    }

    public void OnDisable()
    {
        if(ToolTipSystem.instance.currentTrigger == this)
        {
            ToolTipSystem.Hide();
            ToolTipSystem.instance.currentTrigger = null;
        }
    }

    public void EnableFinger()
    {
        changeToFinger = true;
    }
}
