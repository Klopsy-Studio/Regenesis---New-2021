using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutMonsterRoarState : BattleState
{
    public override void Enter()
    {
        base.Enter();
     
        StartCoroutine(MonsterRoarSequence());
    }

    IEnumerator MonsterRoarSequence()
    {
        owner.SelectTile(owner.levelData.beginPoint);

        float zoomTime = 0f;

        //while(owner.cinemachineCamera.m_Lens.OrthographicSize > 5.5f)
        //{
        //    owner.cinemachineCamera.m_Lens.OrthographicSize = Mathf.Lerp(owner.cinemachineCamera.m_Lens.OrthographicSize, 5.4f, zoomTime);
        //    zoomTime += Time.deltaTime;
        //}

        owner.cinemachineCamera.m_Lens.OrthographicSize = 5.5f;
        yield return new WaitForSeconds(0.5f);

        foreach (Unit u in playerUnits)
        {
            u.GetComponent<PlayerUnit>().animations.unitAnimator.SetTrigger("beginHunt");
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(0.2f);

        owner.SelectTile(owner.enemyUnits[0].currentPoint);

        yield return new WaitForSeconds(0.3f);
        owner.enemyUnits[0].GetComponent<EnemyUnit>().monsterControl.monsterAnimations.SetTrigger("beginCombat");


        yield return new WaitForSeconds(1f);

        MonsterController controller = owner.enemyUnits[0].GetComponent<MonsterController>();

        controller.monsterAnimations.SetBool("idle", false);
        controller.monsterAnimations.SetBool("roar", true);

        ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);

        ActionEffect.instance.Shake(owner.monsterRoar);
        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }




        controller.monsterAnimations.SetBool("idle", true);
        controller.monsterAnimations.SetBool("roar", false);

        owner.timelineUI.gameObject.SetActive(true);
        owner.timelineUI.isActive = true;
        owner.unitStatusUI.gameObject.SetActive(true);
        owner.partyIconParent.gameObject.SetActive(true);


        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.Play("Music");


        //Maybe a hunt begin banner
        owner.ToggleTimeline();

        owner.ChangeState<TutShowslideState>();
    }

  
}
