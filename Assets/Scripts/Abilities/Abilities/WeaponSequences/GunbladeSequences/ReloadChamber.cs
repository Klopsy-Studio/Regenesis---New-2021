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
        user.SpendActionPoints(ability.actionCost);
        user.IncreaseBullets(5);


        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;

    }
}
