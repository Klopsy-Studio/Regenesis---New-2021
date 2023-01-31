using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/SpawnEventAction")]

public class SpawnEventAction : Action
{
    [SerializeField] List<MonsterEvent> events = new List<MonsterEvent>();
    public override void Act(MonsterController controller)
    {
        MonsterEvent eventToSpawn = events[Random.Range(0, events.Count)];
        MonsterEvent e = Instantiate(eventToSpawn);
        e.controller = controller;
        e.transform.parent = null;
        controller.battleController.timelineElements.Add(e);

        OnExit(controller);
    }
}
