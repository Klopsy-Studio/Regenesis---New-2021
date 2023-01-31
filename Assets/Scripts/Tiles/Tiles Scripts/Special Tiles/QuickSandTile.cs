using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSandTile : Tile
{
    int rangeDebuff = 1;
    int originalRange;
    TimelineVelocity originalTimelineVelocity;
    Unit unit;

    PlayerUnit playerUnit;
    public override void OnUnitArrive()
    {

      
       if (content.TryGetComponent(out Unit u))
       {
            if(u.timelineTypes == TimeLineTypes.PlayerUnit)
            {
                unit = u;
                //Guardar la velocidad timeline anterior en una variable temporal 
                originalTimelineVelocity = u.TimelineVelocity;

                //Cambiar la velocidad timeline a la más lenta
                u.TimelineVelocity = TimelineVelocity.VerySlow;
                //u.DebugThings();
                //Cambiar el rango de la unidad a 1 
                playerUnit = u.GetComponent<PlayerUnit>();
                originalRange = playerUnit.weapon.range;
                playerUnit.weapon.range = rangeDebuff;
              
           

            } 
       }
    }

    public override void OnUnitLeave()
    {
        if(unit != null)
        {
            unit.TimelineVelocity = originalTimelineVelocity;
            //unit.DebugThings();
            playerUnit.weapon.range = originalRange;
        }
    }
}
