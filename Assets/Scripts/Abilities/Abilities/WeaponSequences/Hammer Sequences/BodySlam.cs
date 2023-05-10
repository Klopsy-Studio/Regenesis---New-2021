using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Hammer Sequences/Body Slam")]

public class BodySlam : AbilitySequence
{
    [SerializeField] [Range(0, 1)] float abilityScalingWithHealth1;
    [SerializeField] [Range(0, 1)] float abilityScalingWithHealth2;
    [SerializeField] [Range(0, 1)] float abilityScalingWithHealth3;
    [SerializeField] [Range(0, 1)] float abilityScalingWithHealth4;

    public int furyAmount;

    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        CleanTargets();

        playing = true;
        user.currentAbility = ability;
        user.SpendActionPoints(ability.actionCost);

        if (target.GetComponent<Unit>() != null)
        {
            Unit u = target.GetComponent<Unit>();

            //Checking health ranges

            if(user.health <= 100 && user.health >= 75)
            {
                ability.abilityModifier = abilityScalingWithHealth1;
            }

            else if (user.health <= 74 && user.health >= 50)
            {
                ability.abilityModifier = abilityScalingWithHealth2;
            }

            else if (user.health <= 49 && user.health >= 25)
            {
                ability.abilityModifier = abilityScalingWithHealth3;
            }

            else if (user.health <= 24 && user.health >= 0)
            {
                ability.abilityModifier = abilityScalingWithHealth4;
            }

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

        yield return new WaitForSeconds(1f);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
