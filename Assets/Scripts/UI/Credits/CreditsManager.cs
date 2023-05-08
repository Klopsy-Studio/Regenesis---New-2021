using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] Animator creditsAnimations;
    [SerializeField] UnityEvent OnFinishCredits;
    [SerializeField] UnityEvent onStartCredits;


    public void StartAnimation()
    {
        creditsAnimations.SetTrigger("credits");
    }

    public void FinishAnimation()
    {
        creditsAnimations.SetTrigger("idle");
    }
    public void StartCredits()
    {
        onStartCredits.Invoke();
    }

    public void FinishCredits()
    {
        OnFinishCredits.Invoke();
    }
}
