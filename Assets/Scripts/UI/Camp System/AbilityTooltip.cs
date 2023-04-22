using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityTooltip : MonoBehaviour
{
    public TextMeshProUGUI abilityName;
    public ToolTipTrigger toolTipTrigger;

    public void SetAbilityTooltip(string _abilityName, string tooltipText)
    {
        abilityName.SetText(_abilityName);
        toolTipTrigger.content = tooltipText;
    }
}
