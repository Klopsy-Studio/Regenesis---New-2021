using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAbilityDetailsState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.abilityDetailsUI.gameObject.SetActive(true);

        owner.actionSelectionUI.gameObject.SetActive(false);

        owner.abilityDetailsUI.SetUpDetails(owner.currentUnit.weapon.Abilities[owner.attackChosen]);

        owner.abilityDetailsUI.goBackButton.onClick.AddListener(GoBack);
        owner.abilityDetailsUI.useButton.onClick.AddListener(UseAbility);
    }


    public void UseAbility()
    {
        owner.abilityDetailsUI.goBackButton.onClick.RemoveAllListeners();
        owner.abilityDetailsUI.useButton.onClick.RemoveAllListeners();
        owner.abilityDetailsUI.gameObject.SetActive(false);

        owner.ChangeState<UseAbilityState>();
        
    }

    public void GoBack()
    {
        owner.abilityDetailsUI.goBackButton.onClick.RemoveAllListeners();
        owner.abilityDetailsUI.useButton.onClick.RemoveAllListeners();
        owner.abilityDetailsUI.gameObject.SetActive(false);

        owner.ChangeState<SelectAbilityState>();
    }
}
