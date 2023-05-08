using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/New Unit Profile")]
public class UnitProfile : ScriptableObject
{
    public string unitName;

    [Header("UI Sprites")]
    public Sprite unitPortrait;
    public Sprite unitTimelineIcon;
    public Sprite unitPartyIcon;
    public Sprite unitPartyIconDead;
    public Sprite unitDeathTimelineIcon;

    [Header("Equipment")]
    public Weapons unitWeapon;

    public float characterIndex;
    public float characterIconIndex;
    //public WeaponOffset hammerOffset;
    //public WeaponOffset slingShotOffset;

    public float tutTimelinePos;
}

[System.Serializable]
public class WeaponSpriteData
{
    public SpriteRenderer savedWeaponSprite;
    [Space]
    public SpriteRenderer idleCombatSprite;
    [Space]
    public SpriteRenderer attackSprite;
}

