using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Bow Sequences/Bullseye")]

public class Bullseye : AbilitySequence
{
    [SerializeField] BullseyeEvent bullseyeEvent;

    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        playing = true;
        yield return null;
        controller.tileSelectionToggle.MakeTileSelectionSmall();
        controller.SelectTile(controller.currentUnit.tile.pos);
        yield return new WaitForSeconds(0.3f);
        BullseyeEvent e = Instantiate(bullseyeEvent);

        e.unit = user;
        e.target = target;
        e.ability = ability;

        user.currentAbility = ability;
        user.animations.unitAnimator.SetBool("bullseye", true);

        yield return new WaitForSeconds(1.5f);

        if (controller.bowExtraAttack)
        {
            user.SpendActionPoints(ability.actionCost+1);
            e.numberOfAttacks = 2;
        }
        else
        {
            user.SpendActionPoints(ability.actionCost);
            e.numberOfAttacks = 1;
        }

        controller.timelineElements.Add(e);
        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        controller.endTurnInstantly = true;
        playing = false;
    }
}
