using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusList : MonoBehaviour
{
    [SerializeField] GameObject unitStatusPrefab;

    public List<UnitStatus> unitStatusList;

    public void SpawnUnitStatus(List<Unit> list)
    {
        foreach(PlayerUnit p in list)
        {
            UnitStatus status = Instantiate(unitStatusPrefab, transform).GetComponent<UnitStatus>();
            status.SetUnit(p);
            unitStatusList.Add(status);
        }
    }
}
