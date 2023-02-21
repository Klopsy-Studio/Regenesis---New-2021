using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckIfTargetIsMarked : ActionNode
{
    [SerializeField] MonsterAbility abilityToCheck;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        List<Tile> tiles = abilityToCheck.GetAttackTiles(owner.controller);
        List<PlayerUnit> markedTargets = new List<PlayerUnit>();

        foreach(Tile t in tiles)
        {
            if(t.content != null)
            {
                if(t.content.GetComponent<PlayerUnit>()!= null)
                {
                    PlayerUnit p = t.content.GetComponent<PlayerUnit>();

                    if (p.marked)
                    {
                        markedTargets.Add(p);
                    }
                }
            }
        }

        if (markedTargets.Count <= 0)
        {
            return State.Failure;
        }
        else
        {
            owner.controller.target = markedTargets[Random.Range(0, markedTargets.Count)];
            return State.Success;
        }

    }
}
