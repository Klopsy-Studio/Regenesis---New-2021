using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Hammer Sequences/Stampede")]
public class Stampede : AbilitySequence
{
    [SerializeField] int furyAmount;

    public float[] stampedeValueRange;
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        playing = true;
        yield return null;
        Tile t;
        if (target.GetComponent<Unit>())
        {
            t = target.GetComponent<Unit>().tile;
        }
        
        else if (target.GetComponent<BearObstacleScript>())
        {
            t = controller.board.GetTile(target.GetComponent<BearObstacleScript>().pos);
        }
        else
        {
            t = null;
        }

        LineAbilityRange range;

        if (user.GetComponent<LineAbilityRange>()!= null)
        {
            range = user.GetComponent<LineAbilityRange>();
        }
        else
        {
            range = user.gameObject.AddComponent<LineAbilityRange>();
        }

        range.unit = user;
        range.lineDir = user.tile.GetDirections(t);
        range.lineLength = 4;
        range.stopLine = true;

        List<Tile> tiles = range.GetTilesInRange(controller.board);
        int scaling = tiles.Count;
        //Remove the tile the objective is supposed to be

        if(tiles != null)
        {
            if(tiles[tiles.Count-1] != null)
            {
                tiles.RemoveAt(tiles.Count - 1);
            }
        }

        Tile tileToMove;

        if(tiles != null)
        {
            if(tiles.Count-1 >= 0)
            {
                Debug.Log(tiles.Count - 1);
                if (tiles[tiles.Count - 1] != null)
                {
                    tileToMove = tiles[tiles.Count - 1];
                }
                else
                {
                    tileToMove = null;
                }
            }
            else
            {
                tileToMove = null;
            }
            
        }  
        else
        {
            tileToMove = null;
        }

        //Move
        if(tileToMove != null)
        {
            Movement m = user.GetComponent<Movement>();
            tileToMove.prev = user.tile;
            m.StartTraverse(tileToMove, controller.board);
            while (m.moving)
            {
                yield return null;
            }

        }

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        if (tiles != null)
        {
            switch (scaling)
            {
                case 1:
                    ability.abilityModifier = stampedeValueRange[0];
                    break;
                case 2:
                    ability.abilityModifier = stampedeValueRange[1];
                    break;
                case 3:
                    ability.abilityModifier = stampedeValueRange[2];
                    break;
                case 4:
                    ability.abilityModifier = stampedeValueRange[3];
                    break;
                default:
                    ability.abilityModifier = stampedeValueRange[0];
                    break;
            }
        }

        Debug.Log(ability.abilityModifier);

        if(target.GetComponent<Unit>() != null)
        {
            Unit u = target.GetComponent<Unit>();
            Attack(u);

            if (CheckFury())
            {
                HammerFurySequence(5, u, controller, user.tile.GetDirections(u.tile));
                ResetFury();
            }

            else
            {
                IncreaseFury(furyAmount);
            }
        }

        if(target.GetComponent<BearObstacleScript>()!= null)
        {
            user.Attack();
            target.GetComponent<BearObstacleScript>().GetDestroyed(controller.board);

            if (CheckFury())
            {
                ResetFury();
            }

            else
            {
                IncreaseFury(furyAmount);
            }
        }

        user.SpendActionPoints(ability.actionCost);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        //Add fury gain or fury loss
        playing = false;
    }
}
