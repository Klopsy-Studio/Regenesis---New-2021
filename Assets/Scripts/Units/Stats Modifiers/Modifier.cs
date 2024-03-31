using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Modifier
{
    public TypeOfModifier modifierType;
    public int modifierCount;

    public int damageIncrease;
    public int timelineSpeedReduction;
    [Range(0, 1)]
    public float damageReduction;
    public virtual bool SpendModifier()
    {
        modifierCount--;
        if (modifierCount <= 0)
            return true;
        else
            return false;
    }
}


public enum TypeOfModifier
{
    HunterMark, Defense, TimelineSpeed, Damage, Stun, SpiderMark, Bullseye, PiercingSharpness, Antivirus, DroneUnit, SpikyArmor, MinionFrenzy, MinionEvolve, NearDeath
}
