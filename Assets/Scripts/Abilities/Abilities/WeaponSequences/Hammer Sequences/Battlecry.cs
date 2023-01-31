using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Hammer Sequences/Battlecry")]

public class Battlecry : AbilitySequence
{
    public DamageModifier battlecryData;
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        playing = true;

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        AbilityRange r = ability.abilityRange[0].GetOrCreateRange(ability.abilityRange[0].range, user.gameObject);
        r.unit = user;

        List<Tile> tiles = r.GetTilesInRange(controller.board);
        List<PlayerUnit> units = new List<PlayerUnit>();

        foreach(Tile t in tiles)
        {
            if(t.content != null)
            {
                if(t.content.GetComponent<PlayerUnit>() != null)
                {
                    units.Add(t.content.GetComponent<PlayerUnit>());
                }
            }
        }

        foreach(Unit u in units)
        {
            u.defenseModifier.Add(battlecryData);
            u.EnableBattlecry();
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        user.SpendActionPoints(ability.actionCost);
        playing = false;
    }
}
