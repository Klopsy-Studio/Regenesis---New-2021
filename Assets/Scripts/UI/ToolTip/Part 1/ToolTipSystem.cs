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
    public static void Show(string content, string header = "")
    {
        instance.toolTip.SetText(content, header);
        instance.toolTip.gameObject.SetActive(true);
    }
    public static void Hide()
    {
        instance.toolTip.gameObject.SetActive(false);
    }

    public void Prueba()
    {
        DataPersistenceManager.instance.SaveGame();
    }
}
