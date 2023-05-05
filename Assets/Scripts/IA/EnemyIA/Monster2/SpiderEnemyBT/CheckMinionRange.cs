using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckMinionRange : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
        List<Tile> tiles = new List<Tile>();
        foreach (MinionUnit m in owner.controller.minionsInGame)
        {
            List<Tile> dirtyTiles = m.GetMinionAttackArea(owner.controller.battleController.board);

            foreach(Tile t in dirtyTiles)
            {
                if (!tiles.Contains(t))
                {
                    tiles.Add(t);
                }
            }
        }


        foreach(Tile t in tiles)
        {
            if(t.content!= null)
            {
                if(t.content.GetComponent<PlayerUnit>() != null)
                {
                    return State.Success;
                }
            }
        }

        return State.Failure;
    }
}
