using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class ConsumeObstacle : ActionNode
{
    bool playing;
    [SerializeField] int healAmmount = 50;
    protected override void OnStart() {
        playing = true;
        owner.controller.StartCoroutine(Consume());
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
        if (playing)
        {
            return State.Running;
        }
        else
        {
            return State.Success;
        }
    }


    IEnumerator Consume()
    {
        playing = true;
        owner.controller.monsterAnimations.SetBool("bite", true);
        owner.controller.monsterAnimations.SetBool("idle", false);

        owner.controller.chosenObstacle.GetDestroyed(owner.controller.battleController.board);
        //Hardcoded heal, replace when skills are implemented
        owner.controller.currentEnemy.Heal(healAmmount);
        ActionEffect.instance.Play(4, 0.5f, 0.01f, 0.05f);


        while (ActionEffect.instance.play || ActionEffect.instance.recovery)
        {
            yield return null;
        }

        owner.controller.monsterAnimations.SetBool("bite", false);
        owner.controller.monsterAnimations.SetBool("idle", true);

        owner.controller.battleController.board.GetTile(owner.controller.chosenObstacle.pos).content = null;
        owner.controller.obstaclesInGame.Remove(owner.controller.chosenObstacle);
        owner.controller.validObstacles.Remove(owner.controller.chosenObstacle);
        owner.controller.chosenObstacle.gameObject.SetActive(false);

        playing = false;
    }
}
