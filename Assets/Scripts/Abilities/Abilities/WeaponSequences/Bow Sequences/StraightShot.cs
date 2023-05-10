using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Bow Sequences/Straight Shot")]
public class StraightShot : AbilitySequence
{
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        CleanTargets();

        user.currentAbility = ability;
        playing = true;
        yield return null; 

        int numberOfAttacks = DefaultBowAttack(controller);

        if(target != null)
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
                user.animations.unitAnimator.SetFloat("attackIndex", 0f);
                Debug.Log("One Attack");
                break;
            case 2:
                user.animations.unitAnimator.SetTrigger("doubleAttack");
                user.animations.unitAnimator.SetFloat("attackIndex", 0f);
                Debug.Log("Two Attack");
                break;       
            default:
                break;
        }

        yield return new WaitForSeconds(1f);

        if(target != null)
        {
            if (target.GetComponent<BearObstacleScript>() != null)
            {
                BearObstacleScript obstacle = target.GetComponent<BearObstacleScript>();
                obstacle.GetDestroyed(controller.board);
            }
        }
        

        while(ActionEffect.instance.play || ActionEffect.instance.recovery)
        {
            yield return null;
        }

        if(target.GetComponent<PlayerUnit>() != null)
        {
            target.GetComponent<PlayerUnit>().playerUI.DisableHealthBar();
        }
        playing = false;
    }

}
