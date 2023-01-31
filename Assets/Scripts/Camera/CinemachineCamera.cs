using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCamera : MonoBehaviour
{
    bool play;
    bool recovery;

    float currentTime;

    [SerializeField] CinemachineVirtualCamera camera;
    [SerializeField] float zoomDuration;

    float originalCameraSize;
    [SerializeField] float cameraSize;
    void Start()
    {
        originalCameraSize = camera.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (play)
            Effect();

        if (recovery)
            Recovery();
    }

    private void Effect()
    {
        currentTime += zoomDuration * Time.deltaTime; // A variable that adds over time

        camera.m_Lens.OrthographicSize = Mathf.Lerp(camera.m_Lens.OrthographicSize, cameraSize, currentTime); // Lerp the Size of the camera with an animation curve

        // Condition on the effect "play" duration (the recovery duration is not included in this time interval)
        if (currentTime >= zoomDuration)
        {
            currentTime = 0f;
            camera.m_Lens.OrthographicSize = cameraSize;
            play = false;
        }
    }

    public void Recovery()
    {
        currentTime += zoomDuration * Time.deltaTime; // A variable that adds over time

        camera.m_Lens.OrthographicSize = Mathf.Lerp(camera.m_Lens.OrthographicSize, originalCameraSize, currentTime); // Lerp the Size of the camera with an animation curve

        // Condition on the effect "play" duration (the recovery duration is not included in this time interval)
        if (currentTime >= zoomDuration)
        {
            currentTime = 0f;
            camera.m_Lens.OrthographicSize = originalCameraSize;
            recovery = false;
        }
    }

    public void ZoomIn()
    {
        play = true;
    }

    public void ZoomOut()
    {
        recovery = true;
    }
}
