using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyActions
{
    None,
    Move,
    Attack,
    Special
};
public class EnemyController : StateMachine
{
    public BattleController battleController;

    public EnemyUnit currentEnemy;

    public Unit currentTarget;
    public List<PlayerUnit> targets;
    public EnemyActions lastAction = EnemyActions.None;

    public Abilities[] abilities;

    public int attackToUse;

    
    public virtual void StartEnemy()
    {

    }
}
