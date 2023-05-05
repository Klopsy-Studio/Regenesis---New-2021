using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MonsterMove : ActionNode
{
    [SerializeField] MoveType typeOfMove;
    [SerializeField] bool treeRunning;

    int timesStarted;
    protected override void OnStart() {
        treeRunning = true;
        owner.controller.StartCoroutine(MoveAction());
    }


    IEnumerator MoveAction()
    {
        Debug.Log("Moving");
        MonsterController monster = owner.controller;
        Movement m = monster.GetComponent<Movement>();
       //controller.battleController.board.SelectMovementTiles(test);


        monster.monsterAnimations.SetBool("hide", true);
        yield return new WaitForSeconds(1f);

        monster.monsterAnimations.SetBool("hide", false);

        monster.battleController.tileSelectionToggle.MakeTileSelectionBig();
        monster.CallCoroutine(m.SimpleTraverse(monster.tileToMove));
        monster.battleController.SelectTile(monster.tileToMove.pos);
        monster.monsterAnimations.SetBool("appear", true);
        //AudioManager.instance.Play("MonsterMovement");

        yield return new WaitForSeconds(1f);

        monster.monsterAnimations.SetBool("appear", false);
        monster.monsterAnimations.SetBool("idle", true);
        monster.monsterAnimations.SetBool("idle", false);

        monster.currentEnemy.currentPoint = monster.tileToMove.pos;
        monster.currentEnemy.UpdateMonsterSpace(monster.battleController.board);

        monster.currentEnemy.actionDone = true;

        treeRunning = false;
    }
    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
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
