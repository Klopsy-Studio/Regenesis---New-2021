using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTargets : MonoBehaviour
{
    [SerializeField] GameObject abilityTargetPrefab;
    public GameObject parent;
    public BattleController controller;
    public Target selectedTarget;

    public List<Target> currentTargets;

    bool monsterTargetted;

    public bool stopSelection;
    public void CreateTargets(List<Tile> targetTiles)
    {
        monsterTargetted = false;
        foreach(Tile t in targetTiles)
        {
            if (t.occupied && !monsterTargetted)
            {
                SpawnTarget(controller.enemyUnits[0].gameObject, AbilityTargetType.Enemies);
                monsterTargetted = true;
            }

            else if(t.content != null)
            {
                if (t.content.GetComponent<PlayerUnit>())
                {
                    SpawnTarget(t.content, AbilityTargetType.Allies);
                }

                else if (t.content.GetComponent<BearObstacleScript>())
                {
                    SpawnTarget(t.content, AbilityTargetType.Obstacles);
                }
            }
        }
    }

    public void SpawnTarget(GameObject targetAssigned, AbilityTargetType type)
    {
        Target t = Instantiate(abilityTargetPrefab, parent.transform).GetComponent<Target>();
        t.owner = this;
        t.controller = controller;
        t.targetType = type;
        t.targetAssigned = targetAssigned;
        t.SetTarget();
    }

    public void ClearTargets()
    {
        int count = parent.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            parent.transform.GetChild(0).gameObject.SetActive(false);
            parent.transform.GetChild(0).transform.parent = null;
        }

        selectedTarget = null;
        currentTargets.Clear();
    }
}
