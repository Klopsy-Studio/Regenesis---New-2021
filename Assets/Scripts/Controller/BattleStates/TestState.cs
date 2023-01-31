using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Yo lo intento de verdad");
    }
}
