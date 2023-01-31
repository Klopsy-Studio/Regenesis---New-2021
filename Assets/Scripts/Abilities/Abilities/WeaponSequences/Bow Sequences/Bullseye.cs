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

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);
        BullseyeEvent e = Instantiate(bullseyeEvent);

        e.unit = user;
        e.target = target;
        e.ability = ability;

        //Change to charge animation in the future
        user.animations.SetAnimation("attack");

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
