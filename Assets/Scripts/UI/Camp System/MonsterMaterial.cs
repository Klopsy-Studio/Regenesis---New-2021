using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Monster
{
    None,
    PutridBear,
    Monster2,
    Monster3
}

//public enum TypeOfMaterial
//{
//    MetalPiece,
//    Biomass,
//    fur,
//    Fang,
//    claw,
//    RottenTissue,
//    MutationRoot

//}

public enum Rarity
{
    Common = 5,
    Special = 3
}

[CreateAssetMenu(menuName = "MonsterMaterial")]
public class MonsterMaterial: ScriptableObject
{
    public string materialName;
    public Sprite sprite;
    public Monster monster;
    //public TypeOfMaterial material;
    public Rarity rarity;
}
