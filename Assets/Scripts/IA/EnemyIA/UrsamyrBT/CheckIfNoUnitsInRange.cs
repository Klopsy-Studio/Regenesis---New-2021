using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckIfNoUnitsInRange : ActionNode
{
    [SerializeField] List<RangeData> rangeToCheck = new List<RangeData>();
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
        
        List<Tile> tiles = new List<Tile>();

        if(rangeToCheck.Count > 0)
        {
            foreach(RangeData data in rangeToCheck)
            {
                AbilityRange range = data.GetOrCreateRange(data.range, owner.controller.gameObject);
                range.unit = owner.controller.currentEnemy;

                List<Tile> trash = new List<Tile>();
                trash = range.GetTilesInRange(owner.controller.battleController.board);

                foreach(Tile t in trash)
                {
                    if (!tiles.Contains(t))
                    {
                        tiles.Add(t);
                    }
                }
            }

            foreach(Tile t in tiles)
            {
                if(t.content != null)
                {
                    if(t.content.GetComponent<PlayerUnit>()!= null)
                    {
                        return State.Failure;
                    }
                }
            }

            return State.Success;

        }
        else
        {
            Debug.Log("No range was given to check");
            return State.Failure;
        }
    }
}
