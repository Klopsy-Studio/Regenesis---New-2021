using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRoarState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(MonsterRoarSequence());
    }

    IEnumerator MonsterRoarSequence()
    {
        owner.SelectTile(owner.enemyUnits[0].currentPoint);
        yield return new WaitForSeconds(1f);
        MonsterController controller = owner.enemyUnits[0].GetComponent<MonsterController>();

        controller.monsterAnimations.SetBool("idle", false);
        controller.monsterAnimations.SetBool("roar", true);
        AudioManager.instance.Play("MonsterRoar");

        ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        AudioManager.instance.Play("Music");

        while (ActionEffect.instance.recovery)
        {
            yield return null;
        }
        controller.monsterAnimations.SetBool("idle", true);
        controller.monsterAnimations.SetBool("roar", false);

        //Maybe a hunt begin banner?
        owner.timelineUI.isActive = true;
        owner.timelineUI.gameObject.SetActive(true);
        owner.unitStatusUI.gameObject.SetActive(true);
        
        owner.ChangeState<TimeLineState>();
    }
}
