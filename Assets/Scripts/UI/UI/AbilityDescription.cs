using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityDescription : MonoBehaviour
{
    public TextMeshProUGUI abilityDescription;
    public Text abilityRange;
    public Text cost;
    public void AssignData(Abilities ability)
    {
        abilityDescription.SetText(ability.description);
    }

    public void AssignData(Consumables consumable)
    {
        abilityDescription.SetText(consumable.consumableDescription);
    }
}
