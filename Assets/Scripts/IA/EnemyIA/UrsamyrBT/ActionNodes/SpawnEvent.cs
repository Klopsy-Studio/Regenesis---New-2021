using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SpawnEvent : ActionNode
{
    [SerializeField] List<MonsterEvent> events;
    protected override void OnStart() {

        if(events.Count == 0)
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
        
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
        return State.Success;
    }
}
