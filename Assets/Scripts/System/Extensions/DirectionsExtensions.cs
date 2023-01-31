using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionsExtensions 
{
    public static Directions GetDirections(this Tile t1, Tile t2) //Compare to tiles and get the direction relative to those tiles
    {
        Directions yDir = Directions.North;
        bool yAssigned = false;
        Directions xDir = Directions.North;
        bool xAssigned = false;

        if (t1.pos.y < t2.pos.y)
        {
            yDir = Directions.North;
            yAssigned = true;
        }
        if (t1.pos.x > t2.pos.x)
        {
            xDir = Directions.East;
            xAssigned = true;
        }
        if (t1.pos.y > t2.pos.y)
        {
            yDir = Directions.South;
            yAssigned = true;
        }
        if (t1.pos.x < t2.pos.x)
        {
            xDir = Directions.West;
            xAssigned = true;
        }

        if(xAssigned && yAssigned)
        {
            int xDiff;
            int yDiff;

            xDiff = Mathf.Abs(t1.pos.x - t2.pos.x);
            yDiff = Mathf.Abs(t1.pos.y - t2.pos.y);

            if(xDiff > yDiff)
            {
                return xDir;
            }
            else
            {
                return yDir;
            }
        }

        if (xAssigned)
        {
            return xDir;
        }

        if (yAssigned)
        {
            return yDir;
        }

        return Directions.North;
    }

    public static Directions GetOppositeDirection(Directions dir)
    {
        switch (dir)
        {
            case Directions.North:
                return Directions.South;
            case Directions.East:
                return Directions.West;
            case Directions.South:
                return Directions.North;
            case Directions.West:
                return Directions.East;
            default:
                return Directions.East;
        }
    }
    public static bool CheckSpecificDirection(this Tile t1, Tile t2, Directions directionToCheck) //Compare to tiles and get the direction relative to those tiles
    {
        switch (directionToCheck)
        {
            case Directions.North:
                if (t1.pos.y < t2.pos.y)
                {
                    return true;
                }
                return false;
            case Directions.East:
                if (t1.pos.x < t2.pos.x)
                {
                    return true;
                }
                return false;
            case Directions.South:
                if (t1.pos.y > t2.pos.y)
                {
                    return true;
                }
                return false;
            case Directions.West:
                if(t1.pos.x > t2.pos.x)
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }
    public static Vector3 ToEuler (this Directions d) //Remember that an enum start at 0
    {
        return new Vector3(0, (int)d * 90, 0);
    }
}
