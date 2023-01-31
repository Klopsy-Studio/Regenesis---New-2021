using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTimeline : MonoBehaviour
{
    [SerializeField] Animator trapAnimations;
    public void Init(Tile t, BattleController controller)
    {
        TrapUnit trap = new TrapUnit();
        trap.trapObject = this;
        transform.localPosition = t.center;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5f, transform.localPosition.z);
        t.modifiers.Add(trap);
        trap.t = t;
        trap.controller = controller;
    }


    public void SetTrap()
    {
        trapAnimations.SetTrigger("trap");
        Destroy(this.gameObject, 0.5f);
    }
}
