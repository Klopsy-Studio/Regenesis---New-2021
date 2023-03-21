using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Gunblade Sequences/Bullet Party")]

public class BulletParty : AbilitySequence
{
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        playing = true;
        user = controller.currentUnit;
        user.SpendActionPoints(ability.actionCost);

        int numberOfBullets = user.gunbladeAmmoAmount;
        user.SpendBullets(user.gunbladeAmmoAmount);

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        if (target != null)
        {
            if (target.GetComponent<Unit>() != null)
            {
                Unit u = target.GetComponent<Unit>();

                for (int i = 0; i < numberOfBullets; i++)
                {
                    if (u != null)
                    {
                        Attack(u);
                    }
                    yield return new WaitForSeconds(0.2f);
                }
            }
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

        playing = false;

    }
}
