using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "References/New Status Icons")]
public class StatusIconsReferences : ScriptableObject
{
    [Header("Buff Icons")]
    public Sprite defenseBuff;
    public Sprite hastedIcon;
    public Sprite damageBuff;
    public Sprite hunterMarkIcon;
    public Sprite bullseyeIcon;
    public Sprite piercingIcon;
    public Sprite droneIcon;
    public Sprite spikyArmor;


    [Header("Debuff Icons")]
    public Sprite defenseDebuff;
    public Sprite slowedIcon;
    public Sprite damageDebuff;
    public Sprite spiderMarkIcon;
    public Sprite stunnedIcon;

}
