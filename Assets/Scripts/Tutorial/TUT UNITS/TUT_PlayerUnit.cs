using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_PlayerUnit : PlayerUnit
{
    [SerializeField] float timelinePos;
    protected override void Start()
    {
        base.Start();
        timelineFill = timelinePos;

    }
}
