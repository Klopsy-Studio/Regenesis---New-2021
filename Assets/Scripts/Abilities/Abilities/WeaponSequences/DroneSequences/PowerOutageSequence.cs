using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Drone Sequences/Power Outage")]

public class PowerOutageSequence : AbilitySequence
{
    [SerializeField] Modifier defenseDown;
    [SerializeField] Modifier attackDown;

    public override IEnumerator Sequence(List<Tile> tiles, List<Tile> droneTiles, BattleController controller)
    {
        playing = true;
        user = controller.currentUnit;
        CleanTargets();

        user.SpendActionPoints(ability.actionCost);


        bool isDroneActive = droneTiles.Count > 0;

        List<EnemyUnit> targets1 = new List<EnemyUnit>();
        List<EnemyUnit> targets2 = new List<EnemyUnit>();

        foreach (Tile t in tiles)
        {
            if (t.content != null)
            {
                if (t.content.GetComponent<EnemyUnit>() != null)
                {
                    if (!targets1.Contains(t.content.GetComponent<EnemyUnit>()))
                    {
                        targets1.Add(t.content.GetComponent<EnemyUnit>());
                    }
                }
            }

            if (t.occupied)
            {
                if (!targets1.Contains(controller.enemyUnits[0].GetComponent<EnemyUnit>()))
                {
                    targets1.Add(controller.enemyUnits[0].GetComponent<EnemyUnit>());
                }
            }
        }

        if (isDroneActive)
        {
            foreach (Tile t in droneTiles)
            {
                if (t.content != null)
                {
                    if (t.content.GetComponent<EnemyUnit>() != null)
                    {
                        if (!targets2.Contains(t.content.GetComponent<EnemyUnit>()))
                        {
                            targets2.Add(t.content.GetComponent<EnemyUnit>());
                        }
                    }
                }

                if (t.occupied)
                {
                    if (!targets2.Contains(controller.enemyUnits[0].GetComponent<EnemyUnit>()))
                    {
                        targets2.Add(controller.enemyUnits[0].GetComponent<EnemyUnit>());
                    }
                }
            }
        }

        user.animations.unitAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(0.8f);
        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeDuration, ability.shakeDuration);

        foreach (EnemyUnit u in targets1)
        {
            u.droneVFX.SetTrigger("debuffPurple");
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        foreach (EnemyUnit u in targets1)
        {
            u.AddDebuff(defenseDown);
        }
        yield return new WaitForSeconds(0.1f);

        foreach (EnemyUnit u in targets1)
        {
            u.AddDebuff(attackDown);
        }
        yield return new WaitForSeconds(1f);

        if (isDroneActive)
        {
            controller.SelectTile(user.droneUnit.tile.pos);
            yield return new WaitForSeconds(0.5f);
            user.droneUnit.droneIndicator.SetTrigger("action");
            yield return new WaitForSeconds(0.5f);

            ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeDuration, ability.shakeDuration);

            foreach (EnemyUnit u in targets2)
            {
                u.droneVFX.SetTrigger("debuffPurple");
            }

            while (ActionEffect.instance.CheckActionEffectState())
            {
                yield return null;
            }

            foreach (EnemyUnit u in targets2)
            {
                u.AddDebuff(defenseDown);
            }
            yield return new WaitForSeconds(0.1f);

            foreach (EnemyUnit u in targets2)
            {
                u.AddDebuff(attackDown);
            }
        }
        
        playing = false;
    }
}
