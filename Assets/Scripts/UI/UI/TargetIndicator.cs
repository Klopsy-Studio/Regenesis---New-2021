using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] GameObject targetIndicator;


    public void SetTarget(Point targetPosition, float offset)
    {
        targetIndicator.SetActive(true);
        targetIndicator.transform.position = new Vector3(targetPosition.x, offset, targetPosition.y);
    }

    public void DeactivateTarget()
    {
        targetIndicator.SetActive(false);
    }
}
