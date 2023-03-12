using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    [SerializeField] GameObject droneTargetPrefab;
    public BattleController controller;
    List<DroneTarget> targets = new List<DroneTarget>();
    public DroneTarget currentTarget;

    public Color unselectedColor;

    public int droneCost;
    public void CreateTargets(PlayerUnit target)
    {
        DroneTarget d = Instantiate(droneTargetPrefab, transform).GetComponent<DroneTarget>();
        d.owner = this;
        d.user = controller.currentUnit;
        d.SetTarget(target);

        targets.Add(d);
    }

    public void CanChangeDroneTarget()
    {
        if (controller.currentUnit.actionsPerTurn < droneCost)
        {
            foreach(DroneTarget t in targets)
            {
                t.targetImage.color = unselectedColor;
                t.canBeSelected = false;
            }
        }
    }

    public void ClearTargets()
    {
        foreach(DroneTarget d in targets)
        {
            d.transform.parent = null;
            d.gameObject.SetActive(false);
        }

        targets.Clear();
        currentTarget = null;
    }

    public void SetCurrentTarget(DroneTarget target)
    {
        if(currentTarget != null)
        {
            currentTarget.targetSetCheck.SetActive(false);
            currentTarget = null;
        }

        currentTarget = target;
        currentTarget.targetSetCheck.SetActive(true);
    }

    
}
