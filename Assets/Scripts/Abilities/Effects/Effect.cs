using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfEffect
{
    PushUnit, FallBack, AddStunValue,
};

[System.Serializable]
public class Effect 
{
    [HideInInspector] public bool effectPlaying;
    public TypeOfEffect effectType;
    

    public virtual void EnableEffect()
    {

    }
    public virtual void PushUnit(Unit target, Directions dir, Board board)
    {
        effectPlaying = true;
        Movement m = target.GetComponent<Movement>();
        m.PushUnit(dir, 1, board);
    }

    public virtual void AddStunValue(Unit target, float value)
    {
        target.ApplyStunValue(value);
    }
    public void FallBack(Unit target, Directions dir, Board board)
    {
        Directions oppositeDirection = dir;

        switch (dir)
        {
            case Directions.North:
                oppositeDirection = Directions.South;
                break;
            case Directions.East:
                oppositeDirection = Directions.West;
                break;
            case Directions.South:
                oppositeDirection = Directions.North;
                break;
            case Directions.West:
                oppositeDirection = Directions.East;
                break;
            default:
                break;
        }

        target.GetComponent<Movement>().PushUnit(oppositeDirection, 1, board);
    }
}
