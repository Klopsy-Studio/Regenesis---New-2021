using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1State : State
{
    public Monster1Controller owner;
    public EnemyUnit currentEnemy { get { return owner.currentEnemy; } }
    public BattleController battleController { get { return owner.battleController; } }
    public Unit currentTarget { get { return owner.currentTarget; } }
    public Abilities[] abilities { get { return owner.abilities; } }
    public int attackToUse { get { return owner.attackToUse; } }

    protected string stateName;
    public virtual void Awake()
    {
        owner = GetComponent<Monster1Controller>();
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("I've entered " + stateName);
    }

    protected List<Tile> RegularGetTilesInRange(int range)
    {
        Movement enemyMovement = currentEnemy.GetComponent<Movement>();
        enemyMovement.ChangeFilter(TimeLineTypes.PlayerUnit);
        enemyMovement.ChangeRange(range);
        List<Tile> t = enemyMovement.GetTilesInRangeForEnemy(battleController.board, true);
        return t;
    }

    public virtual T GetRange<T>() where T : AbilityRange
    {
        T target = GetComponent<T>();
        if(target == null)
        {
            target = gameObject.AddComponent<T>();
        }

        return target;
    }

}
