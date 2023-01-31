using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Bow Sequences/Healing Splash")]

public class HealingSplash : AbilitySequence
{
    public List<RangeData> dataForRegularUnits;
    public List<RangeData> dataForMonsters;
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        playing = true;
        yield return null;

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        List<Tile> healingSplashArea = GetSplash(target, controller);

        //We spend points here
        int numberOfAttacks = DefaultBowAttack(controller);
        if (target.GetComponent<Unit>() != null)
        {
            Unit unitTarget = target.GetComponent<Unit>();

            for (int i = 0; i < numberOfAttacks; i++)
            {
                Attack(unitTarget);
                controller.board.SelectMovementTiles(healingSplashArea);
                HealInSplash(healingSplashArea, controller);
                yield return new WaitForSeconds(0.7f);
                controller.board.DeSelectDefaultTiles(healingSplashArea);
            }
        }

        if (target.GetComponent<BearObstacleScript>() != null)
        {
            BearObstacleScript obstacle = target.GetComponent<BearObstacleScript>();
            user.Attack();
            controller.board.SelectMovementTiles(healingSplashArea);
            HealInSplash(healingSplashArea, controller);
            obstacle.GetDestroyed(controller.board);

            yield return new WaitForSeconds(0.7f);
            controller.board.DeSelectDefaultTiles(healingSplashArea);
        }
        
        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;

    }

    public List<Tile> GetSplash(GameObject target, BattleController controller)
    {
        List<Tile> tilesInPond = new List<Tile>();
        List<Tile> trash = new List<Tile>();
        AbilityRange range;

        if(target.GetComponent<PlayerUnit>() != null)
        {
            PlayerUnit p = target.GetComponent<PlayerUnit>();

            foreach (RangeData data in dataForRegularUnits)
            {
                range = data.GetOrCreateRange(data.range, target);
                range.unit = p;

                trash = range.GetTilesInRange(controller.board);

                foreach(Tile t in trash)
                {
                    if (!tilesInPond.Contains(t))
                    {
                        tilesInPond.Add(t);
                    }
                }
            }
            
        }

        if(target.GetComponent<EnemyUnit>() != null)
        {
            EnemyUnit u = target.GetComponent<EnemyUnit>();

            foreach (RangeData data in dataForMonsters)
            {
                range = data.GetOrCreateRange(data.range, target);
                range.unit = u;

                trash = range.GetTilesInRange(controller.board);

                foreach (Tile t in trash)
                {
                    if (!tilesInPond.Contains(t))
                    {
                        tilesInPond.Add(t);
                    }
                }
            }

        }

        if(target.GetComponent<BearObstacleScript>() != null)
        {
            BearObstacleScript b = target.GetComponent<BearObstacleScript>();

            foreach (RangeData data in dataForRegularUnits)
            {
                range = data.GetOrCreateRange(data.range, target);
                range.SetStartPos(b.pos);
                trash = range.GetTilesInRange(controller.board);

                foreach (Tile t in trash)
                {
                    if (!tilesInPond.Contains(t))
                    {
                        tilesInPond.Add(t);
                    }
                }
            }
            
        }

        return tilesInPond;
    }

    public void HealInSplash(List<Tile> tiles, BattleController controller)
    {
        bool monsterAdded = false;
        List<Unit> units = new List<Unit>();
        foreach(Tile t in tiles)
        {
            if(t.occupied && !monsterAdded)
            {
                units.Add(controller.enemyUnits[0]);
                monsterAdded = true;
            }

            if (t.content!= null)
            {
                if(t.content.GetComponent<EnemyUnit>() != null && !monsterAdded)
                {
                    units.Add(controller.enemyUnits[0]);
                    monsterAdded = true;
                }

                if(t.content.GetComponent<PlayerUnit>() != null)
                {
                    units.Add(t.content.GetComponent<PlayerUnit>());
                }
            }
        }


        foreach(Unit u in units)
        {
            u.Heal(ability.initialHeal);
        }
    }
}
