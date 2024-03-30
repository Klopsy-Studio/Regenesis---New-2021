using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ToolTipSystem : MonoBehaviour
{
    public static ToolTipSystem instance;
    public ToolTipTrigger currentTrigger;
    public ToolTip toolTip;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        toolTip.gameObject.SetActive(true);
        toolTip.ChangeTooltipMode(false);
    }
    public static void Show(string content, string header = "")
    {
        instance.toolTip.ChangeTooltipMode(true);
        instance.toolTip.SetText(content, header);
    }
    public static void Hide()
    {

        instance.toolTip.ChangeTooltipMode(false);
    }

}
