using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Bow Sequences/Keen Shot")]

public class KeenShot : AbilitySequence
{
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        user.currentAbility = ability;
        playing = true;
        yield return null;

        int numberOfAttacks = DefaultBowAttack(controller);

        if (target != null)
        {
            if (target.GetComponent<Unit>() != null)
            {
                Unit unitTarget = target.GetComponent<Unit>();
                user.currentTarget = unitTarget;
            }
        }

        switch (numberOfAttacks)
        {
            case 1:
                user.animations.unitAnimator.SetTrigger("attack");
                user.animations.unitAnimator.SetFloat("attackIndex", 1f);
                break;
            case 2:
                user.animations.unitAnimator.SetTrigger("doubleAttack");
                user.animations.unitAnimator.SetFloat("attackIndex", 1f);
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(1f);

        if (target != null)
        {
            if (target.GetComponent<BearObstacleScript>() != null)
            {
                BearObstacleScript obstacle = target.GetComponent<BearObstacleScript>();
                obstacle.GetDestroyed(controller.board);
            }
        }


        while (ActionEffect.instance.play || ActionEffect.instance.recovery)
        {
            yield return null;
        }

        if (target != null)
        {
            if (target.GetComponent<Unit>() != null)
            {
                Unit unitTarget = target.GetComponent<Unit>();
                unitTarget.AddDebuff(new Modifier { modifierType = TypeOfModifier.TimelineSpeed });

                if(numberOfAttacks > 1)
                {
                    yield return new WaitForSeconds(0.3f);
                    unitTarget.AddDebuff(new Modifier { modifierType = TypeOfModifier.TimelineSpeed });
                }
            }
        }

        yield return new WaitForSeconds(0.4f);
        playing = false;
    }
}
