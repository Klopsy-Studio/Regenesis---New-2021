using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Consumables/New Trap")]
public class Trap : Consumables
{
    [SerializeField] TrapTimeline trap;


  
    public override bool ApplyConsumable(Unit unit)
    {
        throw new System.NotImplementedException();
    }

    public override bool ApplyConsumable(Tile t, BattleController battleController)
    {
        var b = Instantiate(trap, new Vector3(t.pos.x, 0.5f, t.pos.y), trap.transform.rotation);
        battleController.currentUnit.animations.SetThrow();
        b.Init(t, battleController);
        return true;
    }
}
