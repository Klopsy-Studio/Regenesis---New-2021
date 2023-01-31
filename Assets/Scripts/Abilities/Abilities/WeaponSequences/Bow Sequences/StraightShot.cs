using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Bow Sequences/Straight Shot")]
public class StraightShot : AbilitySequence
{
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        playing = true;
        yield return null; 

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);
        int numberOfAttacks = DefaultBowAttack(controller);
        if (target.GetComponent<Unit>()!= null)
        {
            Unit unitTarget = target.GetComponent<Unit>();
            for (int i = 0; i < numberOfAttacks; i++)
            {
                AudioManager.instance.Play("SlingshotAttack");
                Attack(unitTarget);
                yield return new WaitForSeconds(0.7f);
            }
        }

        if (target.GetComponent<BearObstacleScript>() != null)
        {
            BearObstacleScript obstacle = target.GetComponent<BearObstacleScript>();
            user.Attack();
            obstacle.GetDestroyed(controller.board);
        }

        while(ActionEffect.instance.play || ActionEffect.instance.recovery)
        {
            yield return null;
        }

        playing = false;
    }

}
