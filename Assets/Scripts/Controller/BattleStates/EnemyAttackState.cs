using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : BattleState
{
    List<Tile> tiles;
    Abilities ability;
    public override void Enter()
    {

        base.Enter();
        owner.isTimeLineActive = false;
        Movement mover = owner.currentEnemyUnit.GetComponent<Movement>();
        ability = owner.currentEnemyUnit.abilities[SelectRandomAbility()];
        //mover.range = ability.range;
        mover.ChangeFilter(TimeLineTypes.PlayerUnit);
        tiles = mover.GetTilesInRangeForEnemy(board, true);
        //tiles = mover.GetTilesInRange(board, true);
        StartCoroutine(AttackUnit(owner.currentEnemyUnit.target));
    }

    IEnumerator AttackUnit(Unit target)
    {
        owner.unitStatusUI.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);

        bool isDead;
        if (tiles.Contains(target.tile))
        {
            isDead = false;

            Debug.Log(owner.currentEnemyUnit.tile.GetDirections(target.tile));
            target.GetComponent<Movement>().PushUnit(target.tile.GetDirections(owner.currentEnemyUnit.tile), 1, board);
            CameraShake.instance.Shake(0.2f, 0.3f);


            yield return new WaitForSeconds(0.5f);

            if (isDead)
            {
                target.gameObject.SetActive(false);
                owner.playerUnits.Remove(target);
             
            }

        }

        yield return null;

        if (owner.playerUnits.Count == 0)
        {
            owner.ChangeState<DefeatState>();
        }
        else
        {
            owner.ChangeState<FinishEnemyUnitTurnState>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        ability = null;
        board.DeSelectTiles(tiles);
        Movement mover = owner.currentEnemyUnit.GetComponent<Movement>();
        mover.ResetRange();
        tiles = null;

  
    }

    int SelectRandomAbility()
    {
        var randomAbility = Random.Range(0, owner.currentEnemyUnit.abilities.Length);
        
        return randomAbility;
    }
}
