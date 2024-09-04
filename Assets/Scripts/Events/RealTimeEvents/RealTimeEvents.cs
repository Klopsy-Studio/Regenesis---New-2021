using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RealTimeEvents : TimelineElements
{

    [SerializeField] protected ParticleEffects particleEffects;
    [SerializeField] protected string name;

    public bool playing;
    [HideInInspector] public Board board;
    public Board Board { get { return board; } set { board = value; } }
    public BattleController battleController;

    bool restarting;
    [SerializeField] float restartTime;
    [SerializeField] float minRestartTime;
    [SerializeField] float maxRestartTime;
    protected virtual void Start()
    {

    }

    private void Update()
    {
        if (battleController.isTimeLineActive && restarting && !battleController.pauseTimeline)
        {
            restartTime -= Time.deltaTime;

            if (restartTime <= 0)
            {
                restarting = false;
                restartTime = 0;
                fTimelineVelocity = 12f;
                battleController.timelineUI.ShowTimelineIcon(this);
                InitialSettings();
            }
        }

    }
    public virtual void InitialSettings()
    {
        timelineTypes = TimeLineTypes.RealtimeEvents;
        //fTimelineVelocity = 0;
        board = battleController.board;
    }
    public override bool UpdateTimeLine()
    {
        if (!restarting)
        {
            timelineFill += fTimelineVelocity * Time.deltaTime;
            if (timelineFill >= timelineFull)
            {
                return true;
            }

            return false;
        }

        return false;
        //Debug.Log(gameObject.name + "timelineFill " + timelineFill);
    }

    public abstract void ApplyEffect();

    public void Return()
    {
        
    }
    public void StartRestartTimer()
    {
        timelineFill = 0;
        restarting = true;
        restartTime = Random.Range(minRestartTime, maxRestartTime);

    }


}
