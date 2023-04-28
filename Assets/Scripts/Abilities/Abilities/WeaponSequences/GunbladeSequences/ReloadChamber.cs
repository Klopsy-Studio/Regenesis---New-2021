using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Gunblade Sequences/Reload Chamber")]

public class ReloadChamber : AbilitySequence
{
    // Start is called before the first frame update
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        playing = true;
        user = controller.currentUnit;
        user.currentAbility = ability;
        user.SpendActionPoints(ability.actionCost);
        user.IncreaseBullets(5);

        user.animations.unitAnimator.SetFloat("attackIndex", 0.8f);
        user.animations.unitAnimator.SetTrigger("attack");

        yield return new WaitForSeconds(1.5f);
        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;

    }
}
