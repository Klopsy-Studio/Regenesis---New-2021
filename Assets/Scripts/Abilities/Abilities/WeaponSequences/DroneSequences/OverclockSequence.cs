using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Drone Sequences/Overclock")]

public class OverclockSequence : AbilitySequence
{
    [SerializeField] Modifier overclockBuff;

    public override IEnumerator Sequence(List<Tile> tiles, BattleController controller)
    {
        playing = true;
        user = controller.currentUnit;
        user.SpendActionPoints(ability.actionCost);

        user.AddBuff(overclockBuff);
        user.IncreaseTimelineVelocity(1);

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeDuration, ability.shakeDuration);

        if (user.droneUnit != null)
        {
            user.droneUnit.IncreaseTimelineVelocity(1);
            user.droneUnit.AddBuff(overclockBuff);
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
            p.IncreaseTimelineVelocity(1);
            p.AddBuff(overclockBuff);
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
