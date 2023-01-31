using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModType
{
    Flat = 100,
    PercentAdd = 200,
    PercentMult = 300,
}


public class StatsModifier
{

    public readonly float value;
    public readonly StatModType type;
    public readonly int order;
    public readonly object source; //type object means it can hold anything

    public StatsModifier(float v, StatModType t, int o, object s)
    {
        value = v;
        type = t;
        order = o;
        object source = o;
    }

    //int order and object source are optionals:
    public StatsModifier(float v, StatModType t) : this(v, t, (int)t, null) { } //este segundo constructor automáticamente va a llamar al primer constructor
    public StatsModifier(float v, StatModType t, int o) : this(v, t, o, null) { }
    public StatsModifier(float v, StatModType t, object s) : this(v, t, (int)t, s) { }

}
