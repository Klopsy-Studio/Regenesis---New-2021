using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Decisions/Can Move To Unit")]
public class CanMoveToUnit : Decision
{
    [SerializeField] bool differentTarget;
    public override bool Decide(MonsterController controller)
    {
        foreach(PlayerUnit p in controller.battleController.playerUnits)
        {
            if (differentTarget)
            {
                if(p == controller.target)
                {
                    continue;
                }
            }

            List<Tile> tiles = p.GetSurroundings(controller.battleController.board);

            foreach (Tile t in tiles)
            {
                if (t.CheckSurroundings(controller.battleController.board) != null)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
