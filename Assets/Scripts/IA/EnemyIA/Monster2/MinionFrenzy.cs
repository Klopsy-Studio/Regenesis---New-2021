using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MinionFrenzy : ActionNode
{
    bool treeRunning;
    protected override void OnStart() {
        treeRunning = true;
        owner.controller.StartCoroutine(Frenzy());
    }

    protected override void OnStop() {
    }

    IEnumerator Frenzy()
    {
        owner.controller.monsterAnimations.SetBool("idle", false);
        owner.controller.monsterAnimations.SetBool("roar", true);

        ActionEffect.instance.Play(4.5f, 0.5f, 0.01f, 0.05f);

        foreach (MinionUnit m in owner.controller.minionsInGame)
        {
            m.EnableBattlecry();
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }
        owner.controller.monsterAnimations.SetBool("idle", true);
        owner.controller.monsterAnimations.SetBool("roar", false);

        foreach (MinionUnit m in owner.controller.minionsInGame)
        {
            m.AddBuff(new Modifier { modifierType = TypeOfModifier.Damage });
            m.AddDebuff(new Modifier { modifierType = TypeOfModifier.Defense });
        }

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
