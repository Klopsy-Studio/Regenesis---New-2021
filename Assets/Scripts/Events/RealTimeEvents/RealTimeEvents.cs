using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RealTimeEvents : TimelineElements
{
    
    [SerializeField] protected ParticleEffects particleEffects;
    [SerializeField] protected string name;


    Board board;
    public Board Board { get { return board; } set { board = value; } }
    public BattleController battleController;

    protected virtual void Start()
    {
       
    }

    public virtual void InitialSettings()
    {
        timelineTypes = TimeLineTypes.RealtimeEvents;
        //fTimelineVelocity = 0;
        board = battleController.board;
    }
    public override bool UpdateTimeLine()
    {
        timelineFill += fTimelineVelocity * Time.deltaTime;
        if (timelineFill >= timelineFull)
        {
          
            return true;
        }

        return false;
        //Debug.Log(gameObject.name + "timelineFill " + timelineFill);
    }

    public abstract void ApplyEffect();

  
}
