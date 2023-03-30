using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutShowslideState : BattleState
{
    TUT_BattleController tutOwner;
  
    public override void Enter()
    {
        base.Enter();
        tutOwner = owner as TUT_BattleController;
        tutOwner.tutorialManager.slidesArray[tutOwner.tutorialSlidesIndex].SetActive(true);
    }
    // Start is called before the first frame update
    protected override void OnSelectAction(object sender, InfoEventArgs<int> e)
    {
       
        switch (e.info)
        {
            case 0:
                Debug.Log("el valor es 0");
                UpdateTutorialSlide();
                break;

            case 1:
                Debug.Log("el valor es 1");
                
                UpdateNextState();
               
                break;



        }
    }
    public override void Exit()
    {
        base.Exit();
        tutOwner.tutorialManager.slidesArray[tutOwner.tutorialSlidesIndex].SetActive(false);
    }


    void UpdateTutorialSlide()
    {
 
        tutOwner.tutorialManager.slidesArray[tutOwner.tutorialSlidesIndex].SetActive(false);
        tutOwner.tutorialSlidesIndex++;
        tutOwner.tutorialManager.slidesArray[tutOwner.tutorialSlidesIndex].SetActive(true);
    }

    void UpdateNextState()
    {
        tutOwner.tutorialManager.slidesArray[tutOwner.tutorialSlidesIndex].SetActive(false);
        tutOwner.tutorialSlidesIndex++;

        int nextState = tutOwner.stateIndex; 
       
        if(nextState == 0)
        {
            owner.ChangeState<TUT_TimelineState>();
        }
    }
}
