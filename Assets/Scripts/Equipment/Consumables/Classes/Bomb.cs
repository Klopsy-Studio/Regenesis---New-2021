using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Consumables/New Bomb")]
public class Bomb : Consumables
{
    [SerializeField] BombTimeline bomb;
    public int range;

    public override bool ApplyConsumable(Unit unit)
    {
        throw new System.NotImplementedException();
    }

    public override bool ApplyConsumable(Tile t, BattleController battleController)
    {
        var b = Instantiate(bomb);
        battleController.currentUnit.animations.SetThrow();
        b.Init(battleController, t);
        Debug.Log("Tile es: " + t);
        b.Place(t);
        b.Match();

        return true;
    }

    



}
