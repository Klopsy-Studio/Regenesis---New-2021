using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Playtest : MonoBehaviour
{
    public BattleController controller;
    [HideInInspector]public List<TimelineElements> elements;
    [SerializeField] ActionEffectParameters testParameters;


    public void JumpToUnitTurn(TimelineElements element)
    {
        element.timelineFill = 100;
    }

    public void PlayActionEffect()
    {
        ActionEffect.instance.Play(testParameters);
    }



}

[System.Serializable]
public class ActionEffectParameters
{
    [Range(0, 100)] public float cameraSize = 3f;
    [Range(0, 10)] public float effectDuration = 0.5f;

    public CinemachineImpulseDefinition shakeDefinition;
    public Vector3 shakeVelocity;
    public float shakeForce;
}



