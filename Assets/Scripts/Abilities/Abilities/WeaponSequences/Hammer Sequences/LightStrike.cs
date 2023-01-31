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
        user.SpendActionPoints(ability.actionCost);

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);
        if (target.GetComponent<Unit>() != null)
        {
            Unit u = target.GetComponent<Unit>();

            Attack(u);

            if (CheckFury())
            {
                HammerFurySequence(5, u, controller, user.tile.GetDirections(u.tile));
                ResetFury();
            }
            else
            {
                IncreaseFury(furyAmount);
            }
        }

        if(target.GetComponent<BearObstacleScript>() != null)
        {
            user.Attack();
            target.GetComponent<BearObstacleScript>().GetDestroyed(controller.board);

            if (CheckFury())
            {
                ResetFury();
            }
            else
            {
                IncreaseFury(furyAmount);
            }
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
