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
            Destroy(d.gameObject, 0.2f);
        }

        targets.Clear();
        currentTarget = null;
    }

    public void SetCurrentTarget(DroneTarget target)
    {

        StartCoroutine(SetCurrentTargetSequence(target));
        Debug.Log("Setting new target");
        //if(currentTarget != null)
        //{
        //    currentTarget.targetSetCheck.SetActive(false);
        //    currentTarget.currentTarget.DisableDrone();
        //    currentTarget = null;
        //}

        //currentTarget = target;
        //currentTarget.targetSetCheck.SetActive(true);
        //currentTarget.currentTarget.EnableDrone();

    }

    public void ApplyTarget(DroneTarget t)
    {
        currentTarget = t;
        currentTarget.targetSetCheck.SetActive(true);
    }
    IEnumerator SetCurrentTargetSequence(DroneTarget target)
    {
        controller.actionSelectionUI.gameObject.SetActive(false);
        controller.abilitySelectionUI.gameObject.SetActive(false);

        foreach(DroneTarget t in targets)
        {
            t.gameObject.SetActive(false);
        }

        if (currentTarget != null)
        {
            controller.SelectTile(currentTarget.currentTarget.tile.pos);
            yield return new WaitForSeconds(0.2f);

            currentTarget.targetSetCheck.SetActive(false);
            currentTarget.currentTarget.DisableDrone();
            currentTarget = null;
            yield return new WaitForSeconds(0.5f);
        }

        controller.SelectTile(target.currentTarget.tile.pos);
        yield return new WaitForSeconds(0.2f);

        currentTarget = target;
        currentTarget.targetSetCheck.SetActive(true);
        currentTarget.currentTarget.EnableDrone();

        yield return new WaitForSeconds(0.5f);

        controller.SelectTile(controller.currentUnit.tile.pos);
        controller.actionSelectionUI.gameObject.SetActive(true);
        controller.abilitySelectionUI.gameObject.SetActive(true);

        foreach (DroneTarget t in targets)
        {
            t.gameObject.SetActive(true);
        }

    }


}
