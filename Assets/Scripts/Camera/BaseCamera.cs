using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamera : MonoBehaviour
{
    Camera cam;
    float originalPosition;
    float originalFov;

    float fovTime;
    [SerializeField] float fovDuration;
    float positionTime;
    [SerializeField] float positionDuration;
    bool updatingFov;
    bool updatingPosition;


    [SerializeField] float fov;
    [SerializeField] float position;
    void Start()
    {
        cam = GetComponent<Camera>();
        originalPosition = cam.transform.position.x;
        originalFov = cam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (updatingFov)
        {
            fovTime += Time.deltaTime;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, fovTime / fovDuration);

            if(fovTime >= fovDuration)
            {
                cam.fieldOfView = fov;
                updatingFov = false;
                fovTime = 0;
            }
        }

        if (updatingPosition)
        {
            positionTime += Time.deltaTime;

            cam.transform.position = new Vector3(Mathf.Lerp(cam.transform.position.x, position, positionTime / positionDuration), cam.transform.position.y, cam.transform.position.z);

            if(positionTime >= positionDuration)
            {
                cam.transform.position = new Vector3(position, cam.transform.position.y, cam.transform.position.z);
                positionTime = 0;
                updatingPosition = false;
            }
        }
    }

    public void UpdateFov(float newFov)
    {
        fov = newFov;
        fovTime = 0;
        updatingFov = true;

    }

    public void UpdatePosition(float newPosition)
    {
        position = newPosition;
        updatingPosition = true;
        positionTime = 0;
    }


    public void ResetCamera()
    {
        UpdateFov(originalFov);
        UpdatePosition(originalPosition);
    }



}
