using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class ShowRangeNode : ActionNode
{

    [SerializeField] MonsterAbility rangeToCheck;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        foreach(RangeData r in rangeToCheck.attackRange)
        {
            AbilityRange a = r.GetOrCreateRange(r.range, owner.controller.gameObject);
            a.unit = owner.controller.currentEnemy;
            List<Tile> tiles = a.GetTilesInRange(owner.controller.battleController.board);

            owner.controller.battleController.board.SelectAttackTiles(tiles);
        }
        return State.Success;
    }
}
