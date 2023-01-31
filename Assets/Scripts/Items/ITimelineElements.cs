using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimelineElements 
{
    TimeLineTypes unitType { get; set; }
    //public TimeLineTypes UnitType { get { return unitType; } }
  
    TimelineVelocity currentVelocity { get; set; }
    float timelineVelocity { get; set; }
    float timelineFill { get; set; }
    float timelineFull { get; set; }
    bool isTimelineActive { get; set; }


    void SetTimelineVar();
    float GetActionBarPosition();
   


    //float GetActionBarPosition()
    //{
    //    return Mathf.Clamp01(timelineFill / timelineFull);
    //}

    bool UpdateTimeLine();


 
}
