using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Hammer Sequences/Fierce Sweep")]

public class FierceSweep : AbilitySequence
{
    [SerializeField] int furyAmmount;
    [SerializeField] int pushAmmount;
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        playing = true;
        List<Tile> tiles = new List<Tile>();

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);


        foreach(RangeData r in ability.abilityRange)
        {
            AbilityRange range = r.GetOrCreateRange(r.range, user.gameObject);
            List<Tile> trashTiles = range.GetTilesInRange(controller.board);

            foreach(Tile t in trashTiles)
            {
                if (!tiles.Contains(t))
                {
                    tiles.Add(t);
                }
            }
        }

        List<Unit> units = new List<Unit>();
        List<BearObstacleScript> obstacles = new List<BearObstacleScript>();
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

                if (t.occupied)
                {
                    if (!units.Contains(controller.enemyUnits[0]))
                    {
                        units.Add(controller.enemyUnits[0]);
                    }
                }

                if (t.content.GetComponent<BearObstacleScript>() != null)
                {
                    obstacles.Add(t.content.GetComponent<BearObstacleScript>());
                }
            }
        }

        user.Attack();

        if (!CheckFury())
        {
            if (units.Count > 0)
            {
                foreach (Unit u in units)
                {
                    Directions targetDir = user.tile.GetDirections(u.tile);
                    Attack(u);
                    u.GetComponent<Movement>().PushUnit(targetDir, pushAmmount, controller.board);
                }
            }

            IncreaseFury(furyAmmount);
        }

        else
        {
            if (units.Count > 0)
            {
                foreach (Unit u in units)
                {
                    Directions targetDir = user.tile.GetDirections(u.tile);
                    Attack(u);
                    u.GetComponent<Movement>().PushUnit(targetDir, 5, controller.board);
                }
            }

            ResetFury();
        }

        if(obstacles.Count > 0)
        {
            foreach(BearObstacleScript o in obstacles)
            {
                o.GetDestroyed(controller.board);
            }
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        user.SpendActionPoints(ability.actionCost);
        playing = false;
    }
}
