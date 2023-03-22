using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Drone Sequences/Power Outage")]

public class PowerOutageSequence : AbilitySequence
{
    [SerializeField] Modifier defenseDown;
    [SerializeField] Modifier attackDown;

    public override IEnumerator Sequence(List<Tile> tiles, BattleController controller)
    {
        user = controller.currentUnit;
        playing = true;

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        
        List<Unit> units = new List<Unit>();

        foreach (Tile t in tiles)
        {
            if (t.content != null)
            {
                if (t.content.GetComponent<Unit>() != null)
                {
                    if (!units.Contains(t.content.GetComponent<Unit>()))
                    {
                        units.Add(t.content.GetComponent<Unit>());
                    }
                }
            }

            if (t.occupied)
            {
                if (!units.Contains(controller.enemyUnits[0]))
                {
                    units.Add(controller.enemyUnits[0]);
                }
            }
        }

        foreach (Unit u in units)
        {
            u.AddDebuff(defenseDown);
            u.AddDebuff(attackDown);

            u.power -= (int)(u.power * 0.25f);
            u.defense = (int)(u.defense * 0.25f);
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        user.SpendActionPoints(ability.actionCost);
        playing = false;
    }
}
