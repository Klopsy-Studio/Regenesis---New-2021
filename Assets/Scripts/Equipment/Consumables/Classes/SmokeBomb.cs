using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Consumables/New Smoke Bomb")]

public class SmokeBomb : Consumables
{
    [SerializeField] SmokeBombTimeline smokeBomb;
    public override bool ApplyConsumable(Tile t, BattleController battleController)
    {
        SmokeBombTimeline bomb = Instantiate(smokeBomb);
        bomb.gameObject.SetActive(true);
        battleController.currentUnit.animations.SetThrow();
        bomb.range.tile = t;

        bomb.ApplyEffect(battleController);
        return true;
    }

    public override bool ApplyConsumable(Unit unit)
    {
        return true;
    }

}
