using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayering : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject spriteObject;

    //Just for monster
    [SerializeField] int offsetX;
    [SerializeField] int offsetY;

    private void Start()
    {
        if (transform.position.x < 0)
        {
            offsetX *= -1;
        }

        if (transform.position.y < 0)
        {
            offsetY *= -1;
        }
    }
    public void AdjustLayer()
    {
        int valueX;
        int valueY;
        int xPosition = ((int)transform.position.x + offsetX) * 100;
        int yPosition = ((int)transform.position.z + offsetY);

        if (xPosition < 0)
        {
            valueX = xPosition * -1;
        }
        else
        {
            valueX = xPosition*-1;
        }
        if (yPosition < 0)
        {
            valueY = yPosition *-1;
        }
        else
        {
            valueY = yPosition;
        }

        sprite.sortingOrder = valueX+valueY;
    }

    private void Update()
    {
        AdjustLayer();
    }
}
