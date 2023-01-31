using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stats
{
    public float baseValue;

    public float Value
    {
        get
        {
            if (isDirty || baseValue != lastBaseValue)
            {
                lastBaseValue = baseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        }
    }


    private bool isDirty = true; //recalculate the value or not
    private float _value;
    private float lastBaseValue = float.MinValue;

    private readonly List<StatsModifier> statModifiers; //You cannot modify it except in the constructor


    public Stats()
    {
        statModifiers = new List<StatsModifier>();
    }
    public Stats(float b): this()
    {
        baseValue = b;
     
    }

    private int CompareModiferOrder(StatsModifier a, StatsModifier b)
    {
        if(a.order < b.order)
        {
            return -1;
        }
        else if (a.order > b.order)
        {
            return 1;
        }

        return 0; //if a.order == b.oder
    }

    public void AddModifier(StatsModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModiferOrder);
    }

    public bool RemoveModifiers(StatsModifier mod)
    {
        if (statModifiers.Remove(mod))
        {
            isDirty = true;
            return true;
        }
        return false;
       
    }

    public bool RemoveAllModifiersFromSource(object s)
    {
        bool didRemove = false;
        for (int i = statModifiers.Count -1; i >=0 ; i--) //it appears that the best way to remove an item from a list is to traverse the list in reverse 
        {
            if(statModifiers[i].source == s)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }

        return didRemove;
    }
    private float CalculateFinalValue()
    {
        float finalValue = baseValue;
        float sumPercentAdd = 0;
        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatsModifier mod = statModifiers[i];
            if(mod.type == StatModType.Flat)
            {
                finalValue += mod.value;
            }
            else if (mod.type == StatModType.PercentAdd)
            {
                sumPercentAdd += mod.value;

                if(i + 1 >= statModifiers.Count || statModifiers[i + 1].type != StatModType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if(mod.type == StatModType.PercentMult)
            {
                finalValue *= 1 + mod.value;
            }
           
        }
        return (float)Math.Round(finalValue, 4); //PROBAR A DEBUGGEAR AQUI EL NUMERO;
    }

    public void ResetValue()
    {
        isDirty = true;
        _value = 0;
    }
    
   

}
