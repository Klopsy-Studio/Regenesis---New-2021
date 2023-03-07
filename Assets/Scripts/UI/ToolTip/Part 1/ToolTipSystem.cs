using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem instance;
    public ToolTip toolTip;
    private void Awake()
    {
        instance = this;
    }
    public static void Show(string content, string header = "")
    {
        instance.toolTip.SetText(content, header);
        instance.toolTip.gameObject.SetActive(true);
    }
    public static void Hide()
    {
        instance.toolTip.gameObject.SetActive(false);
    }
}
