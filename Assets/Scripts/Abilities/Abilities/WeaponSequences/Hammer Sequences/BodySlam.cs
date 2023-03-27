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
        playing = true;
        user.SpendActionPoints(ability.actionCost);

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);


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

            Attack(u);

            if (CheckFury())
            {
                HammerFurySequence(5, u, controller, user.tile.GetDirections(u.tile));
                ResetFury();
            }
            else
            {
                IncreaseFury();
            }
        }

        if (target.GetComponent<BearObstacleScript>() != null)
        {
            user.Attack();
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

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
