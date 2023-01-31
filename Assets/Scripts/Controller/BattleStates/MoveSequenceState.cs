using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSequenceState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        owner.currentUnit.isInAction = true;
        owner.isTimeLineActive = false;
        StartCoroutine(Sequence());
    }

    public override void Exit()
    {
        base.Exit();
        owner.currentUnit.isInAction = false;
    }

    IEnumerator Sequence()
    {
        AudioManager.instance.Play("HunterMovement");
        Movement m = owner.currentUnit.GetComponent<Movement>();

        owner.currentUnit.currentPoint = owner.currentTile.pos;
        StartCoroutine(m.SimpleTraverse(owner.currentTile));

        owner.currentUnit.MovementEffect();
        owner.currentUnit.actionDone = true;

        yield return null;
        owner.ChangeState<SelectActionState>();

    }





}
