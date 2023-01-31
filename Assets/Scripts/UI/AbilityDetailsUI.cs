using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDetailsUI : MonoBehaviour
{

    [Header("UI components")]
    [SerializeField] Text abilityName;
    [SerializeField] Image weaponIcon;
    [SerializeField] Text[] abilityDescription;
    public Button goBackButton;
    public Button useButton;
    

    public void SetUpDetails(Abilities ability)
    {
        abilityName.text = ability.abilityName;
        weaponIcon.sprite = ability.weapon.weaponIcon;
        //Un-comment when the icons are applied
        //weaponIcon.sprite = ability.weapon.weaponIcon;
        
    }
}
