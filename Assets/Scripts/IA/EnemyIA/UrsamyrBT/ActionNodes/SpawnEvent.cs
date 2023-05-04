using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SpawnEvent : ActionNode
{
    [SerializeField] List<MonsterEvent> events;

    bool treeRunning;
    protected override void OnStart() {

        treeRunning = true;
        owner.controller.StartCoroutine(Spawn());
    }

    protected override void OnStop() {
    }


    IEnumerator Spawn()
    {
        if (events.Count == 0)
        {
            Debug.Log("No Event to spawn on node " + owner.nodes.IndexOf(this));
        }
        else
        {
            MonsterEvent eventToSpawn = events[Random.Range(0, events.Count)];
            MonsterEvent e = owner.controller.SpawnEvent(eventToSpawn);
            e.AssignVariables();
            e.transform.parent = null;
            owner.controller.battleController.timelineElements.Add(e);
        }


        owner.controller.monsterAnimations.SetBool("idle", false);
        owner.controller.monsterAnimations.SetBool("roar", true);

        ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        owner.controller.monsterAnimations.SetBool("idle", true);
        owner.controller.monsterAnimations.SetBool("roar", false);

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
