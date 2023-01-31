using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : State
{
    public EnemyController owner;
    public EnemyUnit currentEnemy { get { return owner.currentEnemy; } }
    public BattleController battleController { get { return owner.battleController; } }
    public Unit currentTarget { get { return owner.currentTarget; } }
    public Abilities[] abilities { get { return owner.abilities; } }
    public int attackToUse { get { return owner.attackToUse; } }

    string stateName;
    public virtual void Awake()
    {
        owner = GetComponent<EnemyController>();
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("I've entered " +stateName);
    }

    protected List<Tile> RegularGetTilesInRange(int range)
    {
        //Movement enemyMovement = currentEnemy.GetComponent<Movement>();
        //enemyMovement.ChangeFilter(TimeLineTypes.EnemyUnit);
        //enemyMovement.ChangeRange(range);
        //List<Tile> t = enemyMovement.GetTilesInRange(battleController.board);
        return null;
    }

}
