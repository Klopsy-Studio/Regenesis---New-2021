using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Drone Sequences/Overclock")]

public class OverclockSequence : AbilitySequence
{
    [SerializeField] Modifier overclockBuff;

    public override IEnumerator Sequence(List<Tile> tiles, List<Tile> droneTiles, BattleController controller)
    {
        playing = true;
        user = controller.currentUnit;
        CleanTargets();

        user.SpendActionPoints(ability.actionCost);


        bool isDroneActive = droneTiles.Count > 0;

        List<PlayerUnit> targets1 = new List<PlayerUnit>();
        targets1.Add(user);
        List<PlayerUnit> targets2 = new List<PlayerUnit>();

        if (isDroneActive)
        {
            targets2.Add(user.droneUnit);
        }

        foreach (Tile t in tiles)
        {
            if (t.content != null)
            {
                if (t.content.GetComponent<PlayerUnit>() != null)
                {
                    targets1.Add(t.content.GetComponent<PlayerUnit>());
                }

            }
        }

        if (isDroneActive)
        {
            foreach (Tile t in droneTiles)
            {
                if (t.content != null)
                {
                    if (t.content.GetComponent<PlayerUnit>() != null)
                    {
                        targets2.Add(t.content.GetComponent<PlayerUnit>());
                    }

                    targets2.Add(user.droneUnit);
                }
            }
        }

        user.animations.unitAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(0.8f);
        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeDuration, ability.shakeDuration);

        foreach (PlayerUnit u in targets1)
        {
            u.droneVFX.SetTrigger("debuffOrange");
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        foreach (PlayerUnit u in targets1)
        {
            u.IncreaseTimelineVelocity(1);
            u.AddBuff(overclockBuff);
        }

        yield return new WaitForSeconds(1f);

        if (isDroneActive)
        {
            controller.SelectTile(user.droneUnit.tile.pos);
            yield return new WaitForSeconds(0.5f);
            user.droneUnit.droneIndicator.SetTrigger("action");
            yield return new WaitForSeconds(0.5f);

            ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeDuration, ability.shakeDuration);

            foreach (PlayerUnit u in targets2)
            {
                u.droneVFX.SetTrigger("debuffOrange");
            }

            while (ActionEffect.instance.CheckActionEffectState())
            {
                yield return null;
            }

            foreach (PlayerUnit u in targets2)
            {
                u.IncreaseTimelineVelocity(1);

                u.AddBuff(overclockBuff);
            }

            yield return new WaitForSeconds(1f);
        }

        playing = false;
    }
}
