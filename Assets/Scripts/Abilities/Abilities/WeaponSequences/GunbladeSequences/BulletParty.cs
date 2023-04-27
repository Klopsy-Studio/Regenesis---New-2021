using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Gunblade Sequences/Bullet Party")]

public class BulletParty : AbilitySequence
{
    [SerializeField] int[] criticalPercentage;
    [SerializeField] ActionEffectParameters[] effectParameters;
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        playing = true;
        user = controller.currentUnit;
        user.currentAbility = ability;
        user.SpendActionPoints(ability.actionCost);
        int numberOfBullets = user.gunbladeAmmoAmount;
        user.SpendBullets(user.gunbladeAmmoAmount);
        ability.effectDuration = effectParameters[numberOfBullets - 1].effectDuration;

        if (target.GetComponent<Unit>()!= null)
        {
            user.currentTarget = target.GetComponent<Unit>();
        }

        user.animations.unitAnimator.SetFloat("attackIndex", 1f);
        user.animations.unitAnimator.SetFloat("attackPower", numberOfBullets);
        user.animations.unitAnimator.SetTrigger("attack");

        yield return new WaitForSeconds(1f);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        if (target != null)
        {
            if (target.GetComponent<BearObstacleScript>() != null)
            {
                BearObstacleScript b = target.GetComponent<BearObstacleScript>();
                b.GetDestroyed(controller.board);
                user.Attack();
            }
        }


        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        user.criticalPercentage = user.weapon.criticalPercentage;

        playing = false;

    }
}
