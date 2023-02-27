using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainRandomizer : MonoBehaviour
{
    [SerializeField] Animator trainAnimations;
    [SerializeField] int numberOfAnimations;
    [SerializeField] float maxTime;
    [SerializeField] float minTime;

    [SerializeField] float time;
    void Start() 
    {
        SetRandomTime();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        if(time <= 0)
        {
            trainAnimations.SetTrigger(Random.Range(1, numberOfAnimations + 1).ToString());
            SetRandomTime();
        }
    }

    void SetRandomTime()
    {
        time = Random.Range(minTime, maxTime);
    }
}
