using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class TimelinePreview : MonoBehaviour
{
    List<Unit> previewTimelineList = new List<Unit>();
    List<Unit> sortedPreviewList;
    public void CalculateOrder(List<Unit> _unitsInGame)
    {
        previewTimelineList.Clear();
        //Reset previewTime just in case
        foreach (var unit in _unitsInGame)
        {
            unit.previewTime = 0;
        }

        //Set previewTime
        foreach (var unit in _unitsInGame)
        {
            unit.previewTime = UniformLinearMotion_Time(unit);
            previewTimelineList.Add(unit);
        }

        sortedPreviewList = previewTimelineList.OrderBy(o => o.previewTime).ToList();

        for (int i = 0; i < sortedPreviewList.Count; i++)
        {
            Debug.Log("orden es " + i + " " + sortedPreviewList[i].name);
        }
       
        
    }


    float UniformLinearMotion_Time(Unit unit)
    {
        float finalPos = 100;
        float initPos = unit.timelineFill;
        float velocity = unit.fTimelineVelocity;

        float time = (finalPos - initPos) / velocity;

        return time;
    }


   
}
