using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutShowslideState : BattleState
{
    TUT_BattleController tutOwner;
    bool isNextState = false;
    public override void Enter()
    {
        base.Enter();
        isNextState = false;
        owner.isTimeLineActive = false;
        tutOwner = owner as TUT_BattleController;
        Debug.Log("Entra a tutshowSlideState");
        tutOwner.tutorialManager.slidesArray[tutOwner.tutorialSlidesIndex].SetActive(true);
    }
    // Start is called before the first frame update
    //protected override void OnSelectAction(object sender, InfoEventArgs<int> e)
    //{
       
    //    switch (e.info)
    //    {
    //        case 0:
    //            Debug.Log("el valor es 0");
    //            UpdateTutorialSlide();
    //            break;

    //        case 1:
    //            Debug.Log("el valor es 1");
                
    //            UpdateNextState();
                
    //            break;



    //    }
    //}



    public override void Exit()
    {
        base.Exit();
        if (isNextState)
        {
            tutOwner.stateIndex++;
        }

    }
    public void UpdateTutorialSlide() //UNITY BUTTONS
    {
 
        tutOwner.tutorialManager.slidesArray[tutOwner.tutorialSlidesIndex].SetActive(false);
        tutOwner.tutorialSlidesIndex++;
        tutOwner.tutorialManager.slidesArray[tutOwner.tutorialSlidesIndex].SetActive(true);
    }

    public void UpdateNextState() //UNITY BUTTONS
    {
        tutOwner.tutorialManager.slidesArray[tutOwner.tutorialSlidesIndex].SetActive(false);
        tutOwner.tutorialSlidesIndex++;
        isNextState = true;
        int nextState = tutOwner.stateIndex;
      
        if(nextState == 0)
        {
            owner.ChangeState<TUT_TimelineStateFirstHunter>();
        }
        else if(nextState == 1)
        {
            owner.ChangeState<TUT_SelectUnitState>();
        }
        else if(nextState == 2)
        {
            Debug.Log("next state 2");
            owner.ChangeState<TUT_MoveTargeStateOne>();
        }
        else if (nextState == 3)
        {
            Debug.Log("next state 3");
             owner.ChangeState<TUT_UseAbilityStateOne>();
        }
        else if(nextState == 4)
        {
            Debug.Log("next state 4");
            owner.ChangeState<TUT_SelectActionState_Finish>();
        }
        else if(nextState == 5)
        {
            owner.ChangeState<TUT_TimelineStateFirstHunter>();
        }
    }
}
