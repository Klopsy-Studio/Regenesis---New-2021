using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Ability Sequences")]

public class AbilitySequence : ScriptableObject
{
    [HideInInspector] public Abilities ability;
    [HideInInspector] public PlayerUnit user;
    [HideInInspector] public Weapons weapon;
    [HideInInspector] public bool playing;

    [HideInInspector] public bool moving;

    [Header("Bow Variables")]
    [HideInInspector] public bool extraAttack;
    public virtual IEnumerator Sequence(GameObject target, BattleController controller)
    {
        playing = true;
        yield return null;
        playing = false;
    }

    public virtual IEnumerator Sequence(List<Tile> tiles, BattleController controller)
    {
        playing = true;
        yield return null;
        playing = false;
    }

    public virtual IEnumerator Sequence(List<Tile> tiles, List<Tile> droneTiles, BattleController controller)
    {
        playing = true;
        yield return null;
        playing = false;
    }

    public void CleanTargets()
    {
        user.currentTarget = null;
        user.currentTargets.Clear();
    }
    private void OnEnable()
    {
        extraAttack = false;
        playing = false;
    }


    public void Attack(Unit unitTarget)
    {
        unitTarget.ReceiveDamage(ability.CalculateDmg(user, unitTarget), ability.isCritical);
        user.Attack();
    }

    public void AttackWithCrit(Unit unitTarget)
    {
        unitTarget.ReceiveDamage(ability.CalculateDamageWithCrit(user, unitTarget), true);
        user.Attack();
    }
    public int DefaultBowAttack(BattleController controller)
    {
        int numberOfAttacks = 1;

        if (controller.bowExtraAttack)
        {
            numberOfAttacks++;
            controller.currentUnit.SpendActionPoints(ability.actionCost + 1);
        }
        else
        {
            controller.currentUnit.SpendActionPoints(ability.actionCost);
        }
        controller.bowExtraAttack = false;
        return numberOfAttacks;
    }

    public void HammerFurySequence(int pushFury, Unit target, BattleController controller, Directions dir)
    {
        target.GetComponent<Movement>().PushUnit(dir, pushFury, controller.board);
        //Just a value to trigger it
        target.ApplyStunValue(100);
    }

    public void IncreaseFury()
    {
        user.hammerFuryAmount += ability.furyGain;
        user.playerUI.ChangeFuryValue(user.hammerFuryAmount);
        if(user.hammerFuryAmount >= user.hammerFuryMax)
        {
            user.hammerFuryAmount = user.hammerFuryMax;
            user.EnableHammerTrait();
        }
    }
    public void ResetFury()
    {
        user.hammerFuryAmount = 0;
        user.playerUI.ChangeFuryValue(user.hammerFuryAmount);
        user.ResetWeaponTraits();

    }

    public bool CheckFury()
    {
        if (user.hammerFuryAmount >= user.hammerFuryMax)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator MoveInADirection(Directions dir, BattleController controller, int moveDistance)
    {
        moving = true;
        LineAbilityRange range;

        if (user.GetComponent<LineAbilityRange>() != null)
        {
            range = user.GetComponent<LineAbilityRange>();
        }
        else
        {
            range = user.gameObject.AddComponent<LineAbilityRange>();
        }

        range.unit = user;
        range.lineDir = dir;
        range.lineLength = moveDistance;
        range.stopLine = true;

        List<Tile> tiles = range.GetTilesInRange(controller.board);
        //Remove the tile the objective is supposed to be

        if (tiles.Count > 0)
        {
            if (tiles[tiles.Count - 1] != null)
            {
                tiles.RemoveAt(tiles.Count - 1);
            }
        }

        Tile tileToMove;

        if (tiles != null)
        {
            if (tiles.Count - 1 >= 0)
            {
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
        if (tileToMove != null)
        {
            Movement m = user.GetComponent<Movement>();
            tileToMove.prev = user.tile;
            m.StartTraverse(tileToMove, controller.board, tiles);

            while (m.moving)
            {
                yield return null;
            }
        }

        moving = false;
    }
}
