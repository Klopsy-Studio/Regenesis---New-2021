using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapUnit : TileModifier
{
    public TrapTimeline trapObject;
    public BattleController controller;
    public override void Effect(Unit unit)
    {
        unit.Stun();
        trapObject.SetTrap();

        if(unit.GetComponent<PlayerUnit>()!= null)
        {
            PlayerUnit p = unit.GetComponent<PlayerUnit>();

            if(controller.currentUnit != null)
            {
                if(controller.currentUnit == p)
                {
                    controller.ChangeState<FinishPlayerUnitTurnState>();
                }
            }
        }


        if(unit.GetComponent<EnemyUnit>()!= null)
        {
            EnemyUnit e = unit.GetComponent<EnemyUnit>();

            if (controller.currentEnemyUnit != null)
            {
                if (controller.currentEnemyUnit == e)
                {

                    controller.ChangeState<FinishEnemyUnitTurnState>();
                }
            }
        }
    }


    public void Remove()
    {
        t.modifiers.Remove(this);
    }
}
