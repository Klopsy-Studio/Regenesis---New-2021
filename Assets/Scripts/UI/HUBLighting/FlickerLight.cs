using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    [SerializeField] float flickRate;
    float _flickRate;

    [SerializeField] float flickTime;
    float _flickTime;

    bool toggle = true; //True means on False means off
    [SerializeField] Material lightMaterial;
    [SerializeField] Material offMaterial;

    [SerializeField] float maxTime;
    [SerializeField] float minTime;

    [SerializeField] SpriteRenderer sprite;

    float recoveryTime;

    bool recovery;
    void Start()
    {
        _flickRate = flickRate;
        _flickTime = flickTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!recovery)
        {

            _flickTime -= Time.deltaTime;
            _flickRate -= Time.deltaTime;


            if(_flickRate <= 0)
            {
                if (toggle)
                {
                    sprite.material = offMaterial;
                    toggle = false;
                }
                else
                {
                    sprite.material = lightMaterial;
                    toggle = true;

                }

                _flickRate = flickRate;
            }
            
            if(_flickTime <= 0)
            {
                _flickTime = flickTime;

                sprite.material = lightMaterial;

                Recover();
            }
            
        }

        else
        {
            recoveryTime -= Time.deltaTime;

            if(recoveryTime <= 0)
            {
                recovery = false;
                _flickRate = flickRate;
            }
        }
    }

    public void Recover()
    {
        recovery = true;
        recoveryTime = Random.Range(minTime, maxTime);
    }
}
