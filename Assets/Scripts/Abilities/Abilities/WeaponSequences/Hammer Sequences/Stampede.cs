using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Hammer Sequences/Stampede")]
public class Stampede : AbilitySequence
{
    [SerializeField] int furyAmount;

    public float[] stampedeValueRange;

    [SerializeField] ActionEffectParameters rampageTravelEffects;
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        CleanTargets();

        user.currentAbility = ability;
        playing = true;
        Tile t;
        user.SpendActionPoints(ability.actionCost);

        if (target.GetComponent<Unit>())
        {
            t = target.GetComponent<Unit>().tile;
            user.currentTarget = target.GetComponent<Unit>();
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
        range.monsterUse = false;

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
            user.currentParameters = rampageTravelEffects;
            m.StartTraverse(tileToMove, controller.board, tiles);
            user.animations.unitAnimator.SetTrigger("rampage");
            while (m.moving)
            {
                yield return null;
            }

        }

        //ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        if (tiles != null)
        {
            switch (scaling)
            {
                case 1:
                    ability.abilityModifier = stampedeValueRange[0];
                    user.animations.unitAnimator.SetFloat("attackPower", 0f);
                    break;
                case 2:
                    ability.abilityModifier = stampedeValueRange[1];
                    user.animations.unitAnimator.SetFloat("attackPower", 0.2f);
                    break;
                case 3:
                    ability.abilityModifier = stampedeValueRange[2];
                    user.animations.unitAnimator.SetFloat("attackPower", 0.4f);
                    break;
                case 4:
                    ability.abilityModifier = stampedeValueRange[3];
                    user.animations.unitAnimator.SetFloat("attackPower", 1f);
                    break;
                default:
                    ability.abilityModifier = stampedeValueRange[0];
                    user.animations.unitAnimator.SetFloat("attackPower", 0f);
                    break;
            }
        }

        //Debug.Log(ability.abilityModifier);

        if(target.GetComponent<Unit>() != null)
        {
            Unit u = target.GetComponent<Unit>();
            user.currentTarget = u;

            if (CheckFury())
            {
                user.pushAmount = 5;
                user.pushDirections = user.tile.GetDirections(u.tile);

                u.ApplyStunValue(100);
                ResetFury();
            }
            else
            {
                user.pushAmount = 0;
                IncreaseFury();
            }
        }

        if(target.GetComponent<BearObstacleScript>()!= null)
        {
            target.GetComponent<BearObstacleScript>().GetDestroyed(controller.board);

            if (CheckFury())
            {
                ResetFury();
            }

            else
            {
                IncreaseFury();
            }
        }

        user.animations.unitAnimator.SetTrigger("attack");
        user.animations.unitAnimator.SetFloat("attackIndex", 0.4f);

        yield return new WaitForSeconds(0.5f);
        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        //Add fury gain or fury loss
        playing = false;
    }
}
