using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutShowslideState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        var tutOwner = owner as TUT_BattleController;
        tutOwner.tutorialManager.slidesArray[0].SetActive(true);
    }
    // Start is called before the first frame update
    protected override void OnSelectAction(object sender, InfoEventArgs<int> e)
    {

        switch (e.info)
        {
            case 0:
                Debug.Log("el valor es 0");

                owner.ChangeState<TimeLineState>();
                break;

            case 1:
                Debug.Log("el valor es 1");


                break;



        }
    }
}
