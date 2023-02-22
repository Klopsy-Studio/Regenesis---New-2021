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
    }

    public void AssignData(Consumables consumable)
    {
        abilityDescription.text = consumable.consumableDescription;
    }
}
