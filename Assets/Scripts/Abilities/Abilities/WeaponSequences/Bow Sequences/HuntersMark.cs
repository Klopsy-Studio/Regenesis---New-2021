using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Bow Sequences/Hunter's Mark")]
public class HuntersMark : AbilitySequence
{
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        playing = true;
        yield return null;

        //Change to point or hunter's mark animation
        user.Attack();
        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);
        AudioManager.instance.Play("SlingshotAttack");

        if (target.GetComponent<Unit>() != null)
        {
            Unit u = target.GetComponent<Unit>();
            DamageModifier hunterMark = new DamageModifier();

            u.EnableCriticalMark();
            if (controller.bowExtraAttack)
            {
                hunterMark.modifierCount = 2;
                user.SpendActionPoints(ability.actionCost + 1);
            }
            else
            {
                hunterMark.modifierCount = 1;
                user.SpendActionPoints(ability.actionCost);
            }

            u.criticalModifiers.Add(hunterMark);
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
