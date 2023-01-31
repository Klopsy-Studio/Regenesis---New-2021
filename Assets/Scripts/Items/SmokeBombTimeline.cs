using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBombTimeline : MonoBehaviour
{
    public ItemRange range;
    [SerializeField] int decreaseAmmount;
    bool monsterAdded;
    public void ApplyEffect(BattleController controller)
    {
        List<Tile> tiles = range.GetTilesInRange(controller.board);
        List<Unit> units = new List<Unit>();

        foreach(Tile t in tiles)
        {
            if(t.content != null)
            {
                if(t.content.GetComponent<Unit>() != null)
                {
                    units.Add(t.content.GetComponent<Unit>());
                }
            }

            if (t.occupied && !monsterAdded)
            {
                units.Add(controller.enemyUnits[0]);
                monsterAdded = true;
            }
        }

        foreach(Unit u in units)
        {
            if(u.TimelineVelocity == 0)
            {
                continue;
            }

            u.DecreaseTimelineVelocity(decreaseAmmount);
        }
    }
}
