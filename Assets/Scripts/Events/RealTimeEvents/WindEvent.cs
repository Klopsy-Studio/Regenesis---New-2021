using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WindEvent : RealTimeEvents
{
    
    [SerializeField] GameObject windEffect;
    List<Unit> units;
    public int pushStrength;
    public Directions direction;

    [Header("Force Directions")]
    [SerializeField] bool chooseDirection;
    [SerializeField] Directions forcedDirection;
    protected override void Start()
    {
        base.Start();
        windEffect.SetActive(false);
       
    }

    public override void ApplyEffect()
    {
        windEffect.SetActive(true);

        if (chooseDirection)
        {
            direction = forcedDirection;
            switch (direction)
            {
                case Directions.North:
                    windEffect.transform.rotation = Quaternion.Euler(new Vector3(80, 0, 190));

                    break;
                case Directions.East:
                    windEffect.transform.rotation = Quaternion.Euler(new Vector3(80, 0, -88));

                    break;
                case Directions.South:
                    windEffect.transform.rotation = Quaternion.Euler(new Vector3(80, 0, 12));

                    break;
                case Directions.West:
                    windEffect.transform.rotation = Quaternion.Euler(new Vector3(80, 0, 90));

                    break;
                default:
                    break;
            }
        }

        else
        {
            int i = new System.Random().Next(0, 4);

            if (i == 0)
            {
                direction = Directions.East;
                windEffect.transform.rotation = Quaternion.Euler(new Vector3(80, 0, -88));
            }
            else if (i == 1)
            {
                direction = Directions.South;
                windEffect.transform.rotation = Quaternion.Euler(new Vector3(80, 0, 12));
            }
            else if (i == 2)
            {
                direction = Directions.West;
                windEffect.transform.rotation = Quaternion.Euler(new Vector3(80, 0, 90));
            }
            else if (i == 3)
            {
                direction = Directions.North;
                windEffect.transform.rotation = Quaternion.Euler(new Vector3(80, 0, 190));
            }
        }

        timelineFill = 0;
        units = battleController.unitsInGame;
        AudioManager.instance.Play("WindEvent");

        foreach (var unit in units)
        {
            if (unit.isInAction ||unit.isDead) { continue; }
            Movement mover = unit.GetComponent<Movement>();
            
            mover.PushUnit(direction, pushStrength, Board);
        }
      
        fTimelineVelocity = 24;
      
        Invoke("DeactivateWindEffect", 1);

        playing = false;
    }

    void DeactivateWindEffect()
    {
        windEffect.SetActive(false);
    }
}
