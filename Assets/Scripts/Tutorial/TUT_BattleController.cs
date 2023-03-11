using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_BattleController : BattleController
{
    public TutorialManager tutorialManager;
    public override void BeginGame()
    {
        canToggleTimeline = true;
        originalZoomSize = cinemachineCamera.m_Lens.OrthographicSize;
        cinemachineCamera.m_Lens.NearClipPlane = -1f;
        Destroy(placeholderCanvas.gameObject);
        levelData = GameManager.instance.currentMission;
        cameraTest.transparencySortMode = TransparencySortMode.CustomAxis;
        cameraTest.transparencySortAxis = new Vector3(1, 1, 1);
        Debug.Log("IsWorking");
        ChangeState<TutInitState>();
    }

  
}
