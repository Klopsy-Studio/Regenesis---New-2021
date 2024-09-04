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
            Point pos = new Point((int)target.transform.position.x, (int)target.transform.position.z);
            controller.SelectTile(pos);

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

            if (target.GetComponent<BearObstacleScript>() != null)
            {
                BearObstacleScript b = target.GetComponent<BearObstacleScript>();

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

                yield return new WaitForSeconds(0.5f);
                b.GetDestroyed(controller.board);
                //Replace with charge animation
                unit.Attack();
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


    public override bool UpdateTimeLine()
    {
        Debug.Log(target);
        if(!target.gameObject.activeSelf)
        {
            iconTimeline.EnableDisappear();
            elementEnabled = false;
            timelineVelocity = 0;
            unit.currentBullseyeEvent = null;

            foreach(Modifier m in unit.buffModifiers)
            {
                if (m.modifierType == TypeOfModifier.Bullseye)
                {
                    unit.RemoveBuff(m);
                    break;
                }
            }

            unit.animations.unitAnimator.SetBool("bullseye", false);
            unit.animations.SetIdle();
        }
        else
        {
            if (timelineFill >= timelineFull)
            {
                return true;
            }

            timelineFill += fTimelineVelocity * Time.deltaTime;

            return false;
        }

        return false;



    }

}
