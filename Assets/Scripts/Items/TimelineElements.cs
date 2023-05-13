using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum TimeLineTypes
{
    Null,
    PlayerUnit,
    EnemyUnit,
    RealtimeEvents,
    Items,
    PlayerDeath,
    EnemyEvent,
    HunterEvent
}
public enum TimelineVelocity
{
    //None,
    //Quick,
    //VeryQuick,
    //Normal,
    //Slow,
    //VerySlow,


    VerySlow,
    Slow,
    Normal,
    Quick,
    VeryQuick,
    TurboFast,
    Stun,
}
public abstract class TimelineElements : MonoBehaviour
{
    public int actionsPerTurn;
    public virtual int ActionsPerTurn
    {
        get { return actionsPerTurn; }
        set { actionsPerTurn = value; }
    }

   
    public TimeLineTypes timelineTypes;

    [Header("Timelines variables")]
    public TimelineVelocity timelineVelocity = TimelineVelocity.Normal;
    public TimelineVelocity TimelineVelocity
    {
        get { return timelineVelocity; }
        set { timelineVelocity = value; }
    }

    public bool elementEnabled = true;
    public bool hasDescription;
    public ToolTipDrecription description;

    public float fTimelineVelocity;
    public float timelineFill;
    protected const float timelineFull = 100;
    public bool isTimelineActive;

    public Sprite timelineIcon;
    public string eventDescription;
    public TimelineIconUI iconTimeline;
    public float timelineIconIndex;
    public float previewTime;

    public float GetActionBarPosition()
    {
        return Mathf.Clamp01(timelineFill / timelineFull);
    }

    public abstract bool UpdateTimeLine();
}


[System.Serializable]
public class ToolTipDrecription
{
    public string header;
    [TextArea] public string content;
}
