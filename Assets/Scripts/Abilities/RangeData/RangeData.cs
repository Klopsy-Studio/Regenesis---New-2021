using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangeData 
{
    [SerializeField] public TypeOfAbilityRange range;
    [SerializeField] public Directions lineDir;
    [SerializeField] public int lineLength;
    [SerializeField] public bool stopLine;
    [SerializeField] public int lineOffset;
    [SerializeField] public Directions sideDir;
    [SerializeField] public int sideReach;
    [SerializeField] public int sideLength;
    [SerializeField] public int crossLength;
    [SerializeField] public int crossOffset;
    [SerializeField] public int squareReach;
    [SerializeField] public Directions alternateSideDir;
    [SerializeField] public int alternateSideReach;
    [SerializeField] public int alternateSideLength;
    [SerializeField] public int movementRange;
    [SerializeField] public bool removeOrigin;
    [SerializeField] public int itemRange;
    [SerializeField] public bool itemRemoveContent;


    public AbilityRange GetOrCreateRange(TypeOfAbilityRange rangeType, GameObject receiver)
    {
        AbilityRange rangeClass;

        switch (rangeType)
        {
            case TypeOfAbilityRange.Cone:
                //We never use this 
                if (receiver.GetComponent<ConeAbilityRange>() != null)
                {
                    rangeClass = receiver.GetComponent<ConeAbilityRange>();
                }
                else
                {
                    rangeClass = receiver.AddComponent<ConeAbilityRange>();
                }
                break;

            case TypeOfAbilityRange.Constant:
                //We never use this 
                if (receiver.GetComponent<ConstantAbilityRange>() != null)
                {                
                    rangeClass = receiver.GetComponent<ConstantAbilityRange>();
                }
                else
                {
                    rangeClass = receiver.AddComponent<ConstantAbilityRange>();
                }
                break;

            case TypeOfAbilityRange.Infinite:
                if (receiver.GetComponent<InfiniteAbilityRange>() != null)
                {
                    InfiniteAbilityRange range = receiver.GetComponent<InfiniteAbilityRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                else
                {
                    InfiniteAbilityRange range = receiver.AddComponent<InfiniteAbilityRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                break;

            case TypeOfAbilityRange.LineAbility:
                if (receiver.GetComponent<LineAbilityRange>() != null)
                {
                    LineAbilityRange range = receiver.GetComponent<LineAbilityRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                else
                {
                    LineAbilityRange range = receiver.AddComponent<LineAbilityRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                break;
            case TypeOfAbilityRange.SelfAbility:
                if (receiver.GetComponent<SelfAbilityRange>() != null)
                {
                    SelfAbilityRange range = receiver.GetComponent<SelfAbilityRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                else
                {
                    SelfAbilityRange range = receiver.AddComponent<SelfAbilityRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                break;
            case TypeOfAbilityRange.SquareAbility:
                if (receiver.GetComponent<SquareAbilityRange>() != null)
                {
                    SquareAbilityRange range = receiver.GetComponent<SquareAbilityRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                else
                {
                    SquareAbilityRange range = receiver.AddComponent<SquareAbilityRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                break;
            case TypeOfAbilityRange.Side:
                if (receiver.GetComponent<SideAbilityRange>() != null)
                {
                    SideAbilityRange range = receiver.GetComponent<SideAbilityRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                else
                {
                    SideAbilityRange range = receiver.AddComponent<SideAbilityRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                break;
            case TypeOfAbilityRange.AlternateSide:
                if (receiver.GetComponent<AlternateSideRange>() != null)
                {
                    AlternateSideRange range = receiver.GetComponent<AlternateSideRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                else
                {
                    AlternateSideRange range = receiver.AddComponent<AlternateSideRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                break;
            case TypeOfAbilityRange.Cross:
                if (receiver.GetComponent<CrossAbilityRange>() != null)
                {
                    CrossAbilityRange range = receiver.GetComponent<CrossAbilityRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                else
                {
                    CrossAbilityRange range = receiver.AddComponent<CrossAbilityRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                break;
            case TypeOfAbilityRange.Normal:
                if (receiver.GetComponent<MovementRange>() != null)
                {
                    MovementRange range = receiver.GetComponent<MovementRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                else
                {
                    MovementRange range = receiver.AddComponent<MovementRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                break;
            case TypeOfAbilityRange.Item:
                if (receiver.GetComponent<ItemRange>() != null)
                {
                    ItemRange range = receiver.GetComponent<ItemRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }
                else
                {
                    ItemRange range = receiver.AddComponent<ItemRange>();
                    range.AssignVariables(this);
                    rangeClass = range;
                }

                break;
            default:
                rangeClass = null;
                break;
        }

        if(rangeClass != null)
        {
            return rangeClass;
        }
        else
        {
            return null;
        }
        
    }


    //public AbilityRange AssignRangeClass<T>(GameObject receiver)
    //{
    //    AbilityRange rangeClass;
    //    if (receiver.GetComponent<T>() != null)
    //    {
    //        rangeClass = receiver.GetComponent<T>() as AbilityRange;
    //    }
    //    else
    //    {
    //        rangeClass = receiver.AddComponent<T>();
    //    }
    //    rangeClass.AssignVariables(this);
    //    return rangeClass;
    //}
}


