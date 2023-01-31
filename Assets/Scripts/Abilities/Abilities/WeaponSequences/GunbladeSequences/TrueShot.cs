using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Gunblade Sequences/True Shot")]

public class TrueShot : AbilitySequence
{
    [SerializeField] int numberOfShots;


    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        playing = true;
        user = controller.currentUnit;
        user.SpendActionPoints(ability.actionCost);
        user.SpendBullets(ability.ammoCost);

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        if (target.GetComponent<Unit>()!= null)
        {
            Unit u = target.GetComponent<Unit>();

            for (int i = 0; i < numberOfShots; i++)
            {
                Attack(u);
                yield return new WaitForSeconds(0.2f);
            }
        }

        if (target.GetComponent<BearObstacleScript>() != null)
        {
            BearObstacleScript b = target.GetComponent<BearObstacleScript>();
            b.GetDestroyed(controller.board);
            user.Attack();
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
        
    }
}
