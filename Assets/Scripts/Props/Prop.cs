using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public Point pos;
    public int height;
    public PropData data;
    public SpriteRenderer sprite;
    [HideInInspector] public const float stepHeight = 0.25f;
    [HideInInspector] public Vector3 center { get { return new Vector3(pos.x, height * stepHeight, pos.y); } }
    public void Match()
    {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);
        //transform.localScale = new Vector3(1, height * stepHeight, 1);
    }
    public void Load(Point p, int h)
    {
        pos = p;
        height = h;
        Match();
    }

    public void Load(Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }
}
