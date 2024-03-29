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
        CleanTargets();

        user.SpendActionPoints(ability.actionCost);
        user.SpendBullets(ability.ammoCost);
        user.currentAbility = ability;
        //ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        if(target != null)
        {
            if (target.GetComponent<Unit>() != null)
            {
                Unit u = target.GetComponent<Unit>();

                user.currentTarget = u;
            }
        }
        

        if(target != null)
        {
            if (target.GetComponent<BearObstacleScript>() != null)
            {
                BearObstacleScript b = target.GetComponent<BearObstacleScript>();
                b.GetDestroyed(controller.board);
            }
        }

        user.animations.unitAnimator.SetTrigger("attack");

        user.animations.unitAnimator.SetFloat("attackIndex", 0.4f);

        yield return new WaitForSeconds(2f);


        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
        
    }
}
