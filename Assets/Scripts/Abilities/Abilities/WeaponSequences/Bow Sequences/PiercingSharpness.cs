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
        playing = true;

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        AbilityRange r = ability.abilityRange[0].GetOrCreateRange(ability.abilityRange[0].range, user.gameObject);
        r.unit = user;

        int numberOfAttacks = DefaultBowAttack(controller);

        List<Tile> tiles = r.GetTilesInRange(controller.board);
        List<PlayerUnit> units = new List<PlayerUnit>();

        foreach (Tile t in tiles)
        {
            if (t.content != null)
            {
                if (t.content.GetComponent<PlayerUnit>() != null)
                {
                    units.Add(t.content.GetComponent<PlayerUnit>());
                }
            }
        }

        foreach (Unit u in units)
        {          
            u.AddBuff(piercingSharpnessModifer);
            u.criticalDamage = 2f;
            u.criticalPercentage += 25;

            if(numberOfAttacks > 1)
            {
                u.criticalPercentage += 25;
                u.AddBuff(piercingSharpnessModifer);
            }
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
