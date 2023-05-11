using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Drone Sequences/Short Circuit")]

public class ShortCircuitSequence : AbilitySequence
{
    [SerializeField] ActionEffectParameters parameters;
    public override IEnumerator Sequence(List<Tile> tiles, List<Tile> droneTiles, BattleController controller)
    {
        //Assign variables
        playing = true;
        bool isDroneActive = droneTiles.Count > 0;
        user = controller.currentUnit;
        CleanTargets();

        user.currentAbility = ability;
        List<Unit> units1 = new List<Unit>();
        List<Unit> units2 = new List<Unit>();

        List<BearObstacleScript> obstacles1 = new List<BearObstacleScript>();


        //Spend Points
        user.SpendActionPoints(ability.actionCost);

        //Search for targets
        foreach (Tile t in tiles)
        {
            if (t.content != null)
            {
                if (t.content.GetComponent<Unit>() != null)
                {
                    Unit u = t.content.GetComponent<Unit>();

                    if (!units1.Contains(u))
                    {
                        units1.Add(u);
                    }
                }

                if (t.content.GetComponent<BearObstacleScript>() != null)
                {
                    BearObstacleScript o = t.content.GetComponent<BearObstacleScript>();

                    if (!obstacles1.Contains(o))
                    {
                        obstacles1.Add(o);
                    }
                }
            }

            if (t.occupied)
            {
                if (!units1.Contains(controller.enemyUnits[0]))
                {
                    units1.Add(controller.enemyUnits[0]);
                }
            }
        }

        if(droneTiles.Count > 0)
        {
            foreach (Tile t in droneTiles)
            {
                if (t.content != null)
                {
                    if (t.content.GetComponent<Unit>() != null)
                    {
                        Unit u = t.content.GetComponent<Unit>();

                        if (!units2.Contains(u))
                        {
                            units2.Add(u);
                        }
                    }

                    if (t.content.GetComponent<BearObstacleScript>() != null)
                    {
                        BearObstacleScript o = t.content.GetComponent<BearObstacleScript>();

                        if (!obstacles1.Contains(o))
                        {
                            obstacles1.Add(o);
                        }
                    }
                }

                if (t.occupied)
                {
                    if (!units2.Contains(controller.enemyUnits[0]))
                    {
                        units2.Add(controller.enemyUnits[0]);
                    }
                }
            }
        }

        user.animations.unitAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(1f);
        ActionEffect.instance.Play(parameters);
        if (units1.Count > 0)
        {
            foreach (Unit u in units1)
            {
                Attack(u);
                u.droneVFX.SetTrigger("attack");
            }
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        if (isDroneActive)
        {
            user.controller.SelectTile(user.droneUnit.tile.pos);

            user.droneUnit.droneIndicator.SetTrigger("action");
            yield return new WaitForSeconds(1f);
            
            ActionEffect.instance.Play(parameters);

            if (units2.Count > 0)
            {
                foreach (Unit u in units1)
                {
                    Attack(u);

                    u.droneVFX.SetTrigger("attack");

                }
            }
        }
        

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        if (obstacles1.Count > 0)
        {
            foreach (BearObstacleScript b in obstacles1)
            {
                b.GetDestroyed(controller.board);
            }
        }

        playing = false;
    }
   
    
}
