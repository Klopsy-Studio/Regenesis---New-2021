using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    [Multiline]
    public string content;
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipSystem.Show(content, header);
        ToolTipSystem.instance.currentTrigger = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipSystem.Hide();
    }

    public void OnDisable()
    {
        if(ToolTipSystem.instance.currentTrigger == this)
        {
            ToolTipSystem.Hide();
            ToolTipSystem.instance.currentTrigger = null;
        }
    }

}
