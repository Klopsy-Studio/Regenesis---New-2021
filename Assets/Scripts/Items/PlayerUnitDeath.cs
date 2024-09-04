using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitDeath : TimelineElements
{
    public PlayerUnit unit;
    public bool disabled = false;

    bool placeholderSolution;
    private void Start()
    {
        timelineTypes = TimeLineTypes.PlayerDeath;
    }

    public void DisableDeath(BattleController controller)
    {
        elementEnabled = false;
        unit.elementEnabled = false;

        controller.timelineElements.Remove(this);
        //disabled = true;
        timelineFill = 0;
    }
    public override bool UpdateTimeLine()
    {
        if (!disabled)
        {
            if (timelineFill >= timelineFull)
            {
                return true;
            }

            timelineFill += fTimelineVelocity * Time.deltaTime;
            return false;

        }
        else
        {
            if (!placeholderSolution)
            {
                if(iconTimeline != null)
                {
                    iconTimeline.gameObject.SetActive(false);
                    placeholderSolution = true;
                }
            }
            return false;
        }
    }
}
