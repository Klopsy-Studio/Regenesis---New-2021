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
        user.currentAbility = ability;

        if(target.GetComponent<Unit>()!= null)
        {
            Unit u = target.GetComponent<Unit>();

            user.currentTarget = u;
            user.pushDirections = user.tile.GetDirections(u.tile);

            if (CheckFury())
            {
                user.pushAmount = 5;
                u.ApplyStunValue(100);
                ResetFury();
            }
            else
            {
                user.pushAmount = bashStrenght;
                IncreaseFury();
            }
        }

        else if(target.GetComponent<BearObstacleScript>() != null)
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
        user.animations.unitAnimator.SetFloat("attackIndex", 0.2f);

        yield return new WaitForSeconds(0.5f);
        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
