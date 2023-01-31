using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    [SerializeField] CinemachineCamera camera;
    float shakeTimeRemaining;
    float shakeIntensity;
    float shakeFadeTime;

    bool test;

    private void Awake()
    {
        instance = this;
    }

    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            //float xAmount = Random.Range(-1f, 1f) * shakeIntensity;
            //float yAmount = Random.Range(-1f, 1f) * shakeIntensity;

            float xAmount = Random.Range(-shakeIntensity, shakeIntensity);
            float yAmount = Random.Range(-shakeIntensity, shakeIntensity);

            transform.position += new Vector3(xAmount, yAmount, 0f);

            shakeIntensity = Mathf.MoveTowards(shakeIntensity, 0f, shakeFadeTime * Time.deltaTime);
        }
    }

    public void Shake(float intensity, float duration)
    {
        shakeTimeRemaining = duration;
        shakeIntensity = intensity;

        shakeFadeTime = intensity / duration;

        
    }
}
