using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AbilityTargets : MonoBehaviour
{
    [SerializeField] GameObject abilityTargetPrefab;
    public GameObject parent;
    public BattleController controller;
    public Target selectedTarget;
    public TargetIndicator indicator;
    public List<Target> currentTargets;
    public GameObject noTargetText;
    bool monsterTargetted;

    public bool stopSelection;
    public void CreateTargets(List<Tile> targetTiles)
    {
        monsterTargetted = false;
        foreach(Tile t in targetTiles)
        {
            if (t.occupied && !monsterTargetted)
            {
                SpawnTarget(controller.enemyUnits[0].gameObject, AbilityTargetType.BigMonster);
                monsterTargetted = true;
            }

            else if(t.content != null)
            {
                if (t.content.GetComponent<PlayerUnit>())
                {
                    SpawnTarget(t.content, AbilityTargetType.Allies);
                }

                else if (t.content.GetComponent<MinionUnit>())
                {
                    SpawnTarget(t.content, AbilityTargetType.Enemies);
                }
                else if (t.content.GetComponent<BearObstacleScript>())
                {
                    SpawnTarget(t.content, AbilityTargetType.Obstacles);
                }
            }
        }
    }
    
    public void CreateDroneTargets()
    {
        foreach(Unit p in controller.playerUnits)
        {
            if(p.GetComponent<PlayerUnit>() != controller.currentUnit && controller.currentUnit.droneUnit != p.GetComponent<PlayerUnit>() && !p.GetComponent<PlayerUnit>().hasDrone)
            {
                SpawnTarget(p.gameObject, AbilityTargetType.DroneTarget);
            }
        }

        if(currentTargets.Count <= 0)
        {
            CreateCustomMessage("No targets!");
        }
    }
    public void CreateNoTarget()
    {
        GameObject a = Instantiate(noTargetText, parent.transform);
        a.SetActive(true);
    }

    public void CreateCustomMessage(string message)
    {
        GameObject text = Instantiate(noTargetText, parent.transform);
        text.GetComponent<TextMeshProUGUI>().text = message;
        text.gameObject.SetActive(true);
    }
    public void SpawnTarget(GameObject targetAssigned, AbilityTargetType type)
    {
        Target t = Instantiate(abilityTargetPrefab, parent.transform).GetComponent<Target>();
        t.owner = this;
        t.controller = controller;
        t.targetType = type;
        t.targetAssigned = targetAssigned;
        currentTargets.Add(t);
        t.SetTarget();
    }

    public void ClearTargets()
    {
        int count = parent.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            if(parent.transform.GetChild(0).name == "BowTraitButton")
            {
                parent.transform.GetChild(0).SetAsLastSibling();
                continue;
            }
            parent.transform.GetChild(0).gameObject.SetActive(false);
            parent.transform.GetChild(0).transform.parent = null;
        }
        monsterTargetted = false;
        selectedTarget = null;
        currentTargets.Clear();
    }
}
