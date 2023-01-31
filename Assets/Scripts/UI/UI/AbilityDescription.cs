using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDescription : MonoBehaviour
{
    public Text abilityDescription;
    public Text abilityRange;
    public Text cost;
    public void AssignData(Abilities ability)
    {
        abilityDescription.text = ability.description;
        abilityDescription.fontSize = ability.abilityTextFontSize;
        abilityRange.text = ability.rangeData.movementRange.ToString();
        cost.text = ability.actionCost.ToString();
    }

    public void AssignData(Consumables consumable)
    {
        abilityDescription.text = consumable.description;
        abilityRange.text = consumable.itemRange.itemRange.ToString();
        cost.text = 2.ToString();
    }
}
