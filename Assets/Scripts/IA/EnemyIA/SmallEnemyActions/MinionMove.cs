using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MinionMove : ActionNode
{
    bool treeRunning = false;
    protected override void OnStart() {
        treeRunning = true;
        owner.controller.StartCoroutine(Move());
    }

    protected override void OnStop() {
    }


    IEnumerator Move()
    {
        //Missing animations
        MonsterController monster = owner.controller;
        Movement m = monster.GetComponent<Movement>();

        yield return new WaitForSeconds(1f);

        monster.battleController.tileSelectionToggle.MakeTileSelectionBig();
        monster.CallCoroutine(m.SimpleTraverse(monster.tileToMove));
        monster.battleController.SelectTile(monster.tileToMove.pos);
        monster.currentEnemy.MovementEffect();

        yield return new WaitForSeconds(1f);

        monster.currentEnemy.currentPoint = monster.tileToMove.pos;

        treeRunning = false;
    }
    protected override State OnUpdate() {

        if (treeRunning)
        {
            return State.Running;
        }
        else
        {
            return State.Success;
        }
    }
}
