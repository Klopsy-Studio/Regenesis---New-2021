using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullseyeEvent : HunterEvent
{
    public Abilities ability;
    public int numberOfAttacks;
    List<Modifier> bullseyeBuffs = new List<Modifier>();
    public override IEnumerator ApplyEvent(BattleController controller)
    {
        playing = true;
        if (target != null)
        {
            if (target.GetComponent<Unit>() != null)
            {
                Unit u = target.GetComponent<Unit>();
                unit.currentTarget = u;
                unit.currentAbility = ability;

                switch (numberOfAttacks)
                {
                    case 1:
                        unit.animations.unitAnimator.SetBool("bullseye", false);
                        unit.animations.unitAnimator.SetTrigger("bullseyeRelease");
                        
                        break;
                    case 2:
                        unit.animations.unitAnimator.SetBool("bullseye", false);
                        unit.animations.unitAnimator.SetTrigger("doubleAttack");
                        break;

                    default:
                        break;
                }
            }

            if(target!= null)
            {
                if (target.GetComponent<BearObstacleScript>() != null)
                {
                    BearObstacleScript b = target.GetComponent<BearObstacleScript>();

                    b.GetDestroyed(controller.board);

                    //Replace with charge animation
                    unit.Attack();
                }
            }
            
        }

        yield return new WaitForSeconds(1f);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        foreach(Modifier m in unit.buffModifiers)
        {
            if(m.modifierType == TypeOfModifier.Bullseye)
            {
                bullseyeBuffs.Add(m);
            }
        }

        foreach(Modifier m in bullseyeBuffs)
        {
            unit.RemoveBuff(m);
        }
        unit.animations.unitAnimator.SetBool("bullseye", false);
        unit.animations.unitAnimator.SetTrigger("idle");
        playing = false;
    }

   
}
