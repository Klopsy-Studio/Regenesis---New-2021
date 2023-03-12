using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Drone Sequences/Healing Connection")]

public class HealConnectionSequence : AbilitySequence
{
    public override IEnumerator Sequence(List<Tile> tiles, BattleController controller)
    {
        //Assigning variables
        playing = true;
        user = controller.currentUnit;

        //Spending points
        user.SpendActionPoints(ability.actionCost);

        //Action Effect (Placeholder)

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeDuration, ability.shakeDuration);
        //Ability Effect
        user.Heal(ability.initialHeal);

        if (user.droneUnit != null)
        {
            user.droneUnit.Heal(ability.initialHeal);
        }

        List<PlayerUnit> targets = new List<PlayerUnit>();

        foreach(Tile t in tiles)
        {
            if(t.content!= null)
            {
                if(t.content.GetComponent<PlayerUnit>() != null)
                {
                    targets.Add(t.content.GetComponent<PlayerUnit>());
                }
            }
        }


        foreach(PlayerUnit p in targets)
        {
            p.Heal(ability.initialHeal);
        }


        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
