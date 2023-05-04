using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SpikeyArmor : ActionNode
{
    bool treeRunning;
    protected override void OnStart() {
        treeRunning = true;
        owner.controller.StartCoroutine(ApplyBuff());
    }

    protected override void OnStop() {
    }

    IEnumerator ApplyBuff()
    {
        owner.controller.monsterAnimations.SetBool("bite", true);
        owner.controller.monsterAnimations.SetBool("idle", false);

        owner.controller.chosenObstacle.GetDestroyed(owner.controller.battleController.board);
        //Hardcoded heal, replace when skills are implemented
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

        owner.controller.monsterAnimations.SetBool("spike", true);
        owner.controller.monsterAnimations.SetBool("idle", false);

        ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        owner.controller.currentEnemy.AddBuff(new Modifier { modifierType = TypeOfModifier.SpikyArmor });

        owner.controller.monsterAnimations.SetBool("idle", true);
        owner.controller.monsterAnimations.SetBool("spike", false);

        treeRunning = false;
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
