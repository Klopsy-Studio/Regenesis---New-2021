using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Drone Sequences/Short Circuit")]

public class ShortCircuitSequence : AbilitySequence
{
    public override IEnumerator Sequence(List<Tile> tiles, BattleController controller)
    {
        //Assign variables
        playing = true;
        user = controller.currentUnit;

        List<Unit> units = new List<Unit>();
        List<BearObstacleScript> obstacles = new List<BearObstacleScript>();

        //Spend Points
        user.SpendActionPoints(ability.actionCost);

        //Play Action Effect (Placeholder)
        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        //Search for targets
        foreach (Tile t in tiles)
        {
            if(t.content != null)
            {
                if (t.content.GetComponent<Unit>() != null)
                {
                    Unit u = t.content.GetComponent<Unit>();

                    if (!units.Contains(u))
                    {
                        units.Add(u);
                    }
                }

                if (t.content.GetComponent<BearObstacleScript>()!= null)
                {
                    BearObstacleScript o = t.content.GetComponent<BearObstacleScript>();

                    if (!obstacles.Contains(o))
                    {
                        obstacles.Add(o);
                    }
                }
            }
        }

        if (units.Count > 0)
        {
            foreach(Unit u in units)
            {
                Attack(u);
            }
        }

        if(obstacles.Count > 0)
        {
            foreach(BearObstacleScript b in obstacles)
            {
                b.GetDestroyed(controller.board);
            }
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
