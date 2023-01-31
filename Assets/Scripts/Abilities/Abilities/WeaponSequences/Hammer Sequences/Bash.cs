using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Hammer Sequences/Bash")]

public class Bash : AbilitySequence
{
    [SerializeField] int furyAmount;
    [SerializeField] int bashStrenght;
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        playing = true;
        user.SpendActionPoints(ability.actionCost);
        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        if(target.GetComponent<Unit>()!= null)
        {
            Unit u = target.GetComponent<Unit>();
            Attack(u);
            Movement m = u.GetComponent<Movement>();

            if(CheckFury())
            {
                HammerFurySequence(5, u, controller, user.tile.GetDirections(u.tile));
                ResetFury();
            }
            else
            {
                m.PushUnit(user.tile.GetDirections(u.tile), bashStrenght, controller.board);
                IncreaseFury(furyAmount);
            }

            while (m.moving)
            {
                yield return null;
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
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
