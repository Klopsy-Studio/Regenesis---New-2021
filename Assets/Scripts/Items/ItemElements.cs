using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemElements : TimelineElements
{
    public Point currentPoint;
    public Tile tile;
    public override bool UpdateTimeLine()
    {
        throw new System.NotImplementedException();
    }

    public abstract IEnumerator ApplyEffect( BattleController controller);

    public void Apply(BattleController controller)
    {
        StartCoroutine(ApplyEffect(controller));
    }


}
