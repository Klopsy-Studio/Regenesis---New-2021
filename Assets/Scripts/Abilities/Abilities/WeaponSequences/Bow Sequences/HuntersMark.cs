using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Bow Sequences/Hunter's Mark")]
public class HuntersMark : AbilitySequence
{
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        user.currentTarget = target.GetComponent<Unit>();
        user.currentAbility = ability;
        playing = true;
        yield return null;

        //Change to point or hunter's mark animation
        Modifier hunterMark = new Modifier();

        if (target.GetComponent<Unit>() != null)
        {
            Unit u = target.GetComponent<Unit>();
            hunterMark.modifierType = TypeOfModifier.Critical;

            if (controller.bowExtraAttack)
            {
                hunterMark.modifierCount = 2;
            }
            else
            {
                hunterMark.modifierCount = 1;
            }

        }
        int numberOfAttacks = DefaultBowAttack(controller);


        switch (numberOfAttacks)
        {
            case 1:
                user.animations.unitAnimator.SetTrigger("attack");
                user.animations.unitAnimator.SetFloat("attackIndex", 0.4f);

                break;
            case 2:
                user.animations.unitAnimator.SetTrigger("doubleAttack");
                user.animations.unitAnimator.SetFloat("attackIndex", 0.4f);
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(2f);
        target.GetComponent<Unit>().AddDebuff(hunterMark);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
