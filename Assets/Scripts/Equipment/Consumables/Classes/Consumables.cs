using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
    NormalConsumable,
    TimelineConsumable,
    TargetConsumable,
}
public abstract class Consumables : ScriptableObject
{
    
    public string itemName;
    public Sprite iconSprite;
    public Sprite itemSprite;
    [SerializeField] private ConsumableType consumableType;
    public RangeData itemRange;
    public RangeData effectRange;
    public int maxBackPackAmount;
    public List<AbilityTargetType> elementsToTarget;
    [TextArea]
    public string consumableDescription;
    public ConsumableType ConsumableType { get { return consumableType; } }

  
   
    public abstract bool ApplyConsumable(Unit unit);
    public abstract bool ApplyConsumable(Tile t, BattleController battleController);

    public virtual bool CanUseItem()
    {
        return true;
    }

}
