using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullseyeEvent : HunterEvent
{
    public Abilities ability;
    public int numberOfAttacks;

    public override IEnumerator ApplyEvent(BattleController controller)
    {
        playing = true;
        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        if (target.GetComponent<Unit>() != null)
        {
            Unit u = target.GetComponent<Unit>();
            for (int i = 0; i < numberOfAttacks; i++)
            {
                u.ReceiveDamage(ability.CalculateDmg(unit, u));

                //Replace with charge animation
                unit.Attack();
                yield return new WaitForSeconds(0.7f);
            }
        }

        if(target.GetComponent<BearObstacleScript>() != null)
        {
            BearObstacleScript b = target.GetComponent<BearObstacleScript>();

            b.GetDestroyed(controller.board);

            //Replace with charge animation
            unit.Attack();
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }
        unit.animations.SetAnimation("idle");
        playing = false;
    }

   
}
