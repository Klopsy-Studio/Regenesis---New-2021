using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Bow Sequences/Piercing Sharpness")]

public class PiercingSharpness : AbilitySequence
{
    [SerializeField] Modifier piercingSharpnessModifer;
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        CleanTargets();

        playing = true;
        user.currentAbility = ability;
    
        AbilityRange r = ability.abilityRange[0].GetOrCreateRange(ability.abilityRange[0].range, user.gameObject);
        r.unit = user;

        int numberOfAttacks = DefaultBowAttack(controller);

        List<Tile> tiles = r.GetTilesInRange(controller.board);
        List<GameObject> unitsObjects = new List<GameObject>();

        foreach (Tile t in tiles)
        {
            if (t.content != null)
            {
                if (t.content.GetComponent<PlayerUnit>() != null)
                {
                    unitsObjects.Add(t.content);
                }
            }
        }

        user.currentTargets = unitsObjects;
        user.animations.unitAnimator.SetFloat("attackIndex", 0.6f);
        user.animations.unitAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(1f);
        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }


        foreach (GameObject o in unitsObjects)
        {
            Unit u = o.GetComponent<Unit>();
            u.AddBuff(piercingSharpnessModifer);
            u.criticalDamage = 2f;
            u.criticalPercentage += 25;
        }

        yield return new WaitForSeconds(0.4f);
        if (numberOfAttacks > 1)
        {
            foreach (GameObject o in unitsObjects)
            {
                Unit u = o.GetComponent<Unit>();
                u.AddBuff(piercingSharpnessModifer);
                u.criticalDamage = 2f;
                u.criticalPercentage += 25;
            }
        }

        yield return new WaitForSeconds(0.5f);

        playing = false;
    }
}
