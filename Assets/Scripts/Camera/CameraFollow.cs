using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] bool lerp = false;
    [SerializeField] [Range(1, 10)] float smoothFactor;


    // Update is called once per frame
    void LateUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (lerp)
        {
            Vector3 targetPosition = target.position + offset;
            Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.deltaTime);

            transform.position = smoothPosition;
        }
        else
        {
            transform.position = target.position + offset;
        }
    }
}
