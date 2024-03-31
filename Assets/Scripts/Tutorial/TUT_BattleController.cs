using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_BattleController : BattleController
{
    public TutorialManager tutorialManager;
    public int tutorialSlidesIndex;
    public int stateIndex;
    public override void BeginGame()
    {
        Debug.Log("Begin");
        sceneTransition.SetBool("fadeOut", true);
        canToggleTimeline = true;
        originalZoomSize = cinemachineCamera.m_Lens.OrthographicSize;
        cinemachineCamera.m_Lens.NearClipPlane = -1f;
        Destroy(placeholderCanvas.gameObject);
        cameraTest.transparencySortMode = TransparencySortMode.CustomAxis;
        cameraTest.transparencySortAxis = new Vector3(1, 1, 1);
        ChangeState<TutInitState>();
    }

    public override void Start()
    {
        zoomed = false;
        currentEntity = CurrentEntityTurn.None;
        isTimeLineActive = false;
    }


    //public void ChangeStateIndex(int index)
    //{
    //    stateIndex = index;
    //}
    //public void SumTutorialSlideIndex()
    //{
    //    tutorialSlidesIndex++;
    //}

}
