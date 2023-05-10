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
        CleanTargets();

        playing = true;
        List<Tile> tiles = new List<Tile>();

        user.currentAbility = ability;

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
                    if (!user.currentTargets.Contains(t.content) && user.gameObject != t.content)
                    {
                        user.currentTargets.Add(t.content);
                    }
                }

                if (t.content.GetComponent<BearObstacleScript>() != null)
                {
                    if (!user.currentTargets.Contains(t.content))
                    {
                        user.currentTargets.Add(t.content);
                    }
                }
            }

            if (t.occupied)
            {
                if (!user.currentTargets.Contains(controller.enemyUnits[0].gameObject))
                {
                    user.currentTargets.Add(controller.enemyUnits[0].gameObject);
                }
            }
        }

        if (CheckFury())
        {
            user.pushAmount = 5;

            foreach(GameObject o in user.currentTargets)
            {
                o.GetComponent<Unit>().ApplyStunValue(100);
            }

            ResetFury();
        }
        else
        {
            user.pushAmount = 1;
            IncreaseFury();
        }

        user.animations.unitAnimator.SetFloat("attackIndex", 0.8f);
        user.animations.unitAnimator.SetTrigger("attack");

        yield return new WaitForSeconds(2f);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }


        user.SpendActionPoints(ability.actionCost);
        playing = false;
    }
}
