using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Drone Sequences/Power Supply")]

public class PowerSupplySequence : AbilitySequence
{
    [SerializeField] Modifier powerSupplyBuff;

    public override IEnumerator Sequence(List<Tile> tiles, BattleController controller)
    {
        playing = true;
        user = controller.currentUnit;
        user.SpendActionPoints(ability.actionCost);

        user.AddBuff(powerSupplyBuff);

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeDuration, ability.shakeDuration);

        if (user.droneUnit != null)
        {
            user.droneUnit.AddBuff(powerSupplyBuff);
        }

        List<PlayerUnit> targets = new List<PlayerUnit>();

        foreach (Tile t in tiles)
        {
            if (t.content != null)
            {
                if (t.content.GetComponent<PlayerUnit>() != null)
                {
                    targets.Add(t.content.GetComponent<PlayerUnit>());
                }
            }
        }

        foreach (PlayerUnit p in targets)
        {
            p.AddBuff(powerSupplyBuff);
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
