using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterEvent : TimelineElements
{
    public GameObject target;
    public PlayerUnit unit;
    public bool playing = false;
    public override bool UpdateTimeLine()
    {
        if (timelineFill >= timelineFull)
        {
            return true;
        }

        timelineFill += fTimelineVelocity * Time.deltaTime;
        return false;
    }

    public void Apply(BattleController controller)
    {
        StartCoroutine(ApplyEvent(controller));
    }
    public virtual IEnumerator ApplyEvent(BattleController controller)
    {
        yield return null;
    }
}
