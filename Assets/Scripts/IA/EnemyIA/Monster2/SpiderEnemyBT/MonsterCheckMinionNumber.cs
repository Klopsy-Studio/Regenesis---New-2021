using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MonsterCheckMinionNumber : ActionNode
{
    [SerializeField] ComparisonType checkType;
    [SerializeField] int numberToCheck;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        switch (checkType)
        {
            case ComparisonType.Equals:
                if(numberToCheck == owner.controller.minionsInGame.Count)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.Greater:
                if (owner.controller.minionsInGame.Count > numberToCheck)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.Lower:
                if (owner.controller.minionsInGame.Count < numberToCheck)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }

            case ComparisonType.GreaterAndEqual:
                if (numberToCheck >= owner.controller.minionsInGame.Count)
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }
            case ComparisonType.LowerAndEqual:
                if (owner.controller.minionsInGame.Count <= numberToCheck  )
                {
                    return State.Success;
                }
                else
                {
                    return State.Failure;
                }

            default:
                return State.Success;
        }

    }
}

public enum ComparisonType
{
    Equals, Greater, GreaterAndEqual, Lower, LowerAndEqual
};
