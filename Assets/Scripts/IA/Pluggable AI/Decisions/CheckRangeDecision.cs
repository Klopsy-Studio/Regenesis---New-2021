using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/CheckRangeDecision")]
public class CheckRangeDecision : Decision
{
    public override bool Decide(MonsterController controller)
    {
        return CheckToMoveClosestUnit(controller);
    }

    bool CheckToMoveClosestUnit(MonsterController controller)
    {
        bool isThereAnyUnit = false;

      
        SquareAbilityRange check1Range = controller.GetRange<SquareAbilityRange>();
        check1Range.unit = controller.currentEnemy;
        SideAbilityRange attack2Range = controller.GetRange<SideAbilityRange>();
        attack2Range.unit = controller.currentEnemy;


        List<Tile> tiles = check1Range.GetTilesInRange(controller.battleController.board);

        foreach (Tile t in tiles)
        {
            if (t.content != null)
            {
                //If there is an enemy in range 1, we check if there are more than 2 enemies in this range
                if (t.content.GetComponent<PlayerUnit>() != null)
                {
                    controller.target = t.content.GetComponent<PlayerUnit>();

                    isThereAnyUnit = true;
                  
                }
                else
                {
                    isThereAnyUnit = false;
                }
            }
            else
            {
                isThereAnyUnit = false;
            }
        }

        Debug.Log("ISTHERE ANYUNIT, " + isThereAnyUnit);
        //return isThereAnyUnit;
        return true;
    }




   
}
