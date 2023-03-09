using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Gunblade Sequences/True Shot")]

public class TrueShot : AbilitySequence
{
    [SerializeField] int numberOfShots;


    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        playing = true;
        user.currentAbility = ability;

        if (target != null)
        {
            if (target.GetComponent<Unit>() != null)
            {
                user.currentTarget = target.GetComponent<Unit>();
            }
        }

        
        user.SpendActionPoints(ability.actionCost);
        user.SpendBullets(ability.ammoCost);
        user.animations.unitAnimator.SetFloat("attackIndex", 0.4f);
        user.animations.unitAnimator.SetTrigger("attack");

        yield return new WaitForSeconds(0.5f);
        //if (target != null)
        //{
        //    if (target.GetComponent<Unit>() != null)
        //    {
        //        Unit u = target.GetComponent<Unit>();

        //        for (int i = 0; i < numberOfShots; i++)
        //        {
        //            if (u != null)
        //            {
        //                Attack(u);
        //            }
        //            yield return new WaitForSeconds(0.2f);
        //        }
        //    }
        //}
        

        if(target != null)
        {
            if (target.GetComponent<BearObstacleScript>() != null)
            {
                BearObstacleScript b = target.GetComponent<BearObstacleScript>();
                b.GetDestroyed(controller.board);
            }
        }
        

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
        
    }
}
