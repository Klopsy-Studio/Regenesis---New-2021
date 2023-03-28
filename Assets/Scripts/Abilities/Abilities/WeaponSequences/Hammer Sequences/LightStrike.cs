using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Hammer Sequences/Light Strike")]

public class LightStrike : AbilitySequence
{
    [SerializeField] int furyAmount;

    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        playing = true;
        user.currentAbility = ability;
        user.SpendActionPoints(ability.actionCost);

        //ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);
        if (target.GetComponent<Unit>() != null)
        {
            Unit u = target.GetComponent<Unit>();
            user.currentTarget = u;

            if (CheckFury())
            {
                user.pushAmount = 5;
                user.pushDirections = user.tile.GetDirections(u.tile);
                u.ApplyStunValue(100);
                ResetFury();
            }
            else
            {
                user.pushAmount = 0;
                IncreaseFury();
            }
        }

        

        else if (target.GetComponent<BearObstacleScript>() != null)
        {
            target.GetComponent<BearObstacleScript>().GetDestroyed(controller.board);

            if (CheckFury())
            {
                ResetFury();
            }
            else
            {
                IncreaseFury();
            }
        }

        user.animations.unitAnimator.SetTrigger("attack");
        user.animations.unitAnimator.SetFloat("attackIndex", 0f);

        yield return new WaitForSeconds(2f);
        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
