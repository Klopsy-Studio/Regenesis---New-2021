using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnitState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        //StartCoroutine(CanAct());
        StartCoroutine(SelectUnitCoroutine());
    }

    IEnumerator SelectUnitCoroutine()
    {
        owner.isTimeLineActive = false;
        owner.turnStatusUI.IndicateTurnStatus(owner.turnStatusUI.playerTurn);
        yield return new WaitForSeconds(1f);
        owner.turnStatusUI.StopTurnStatus();
        yield return new WaitForSeconds(1f);

        SelectTile(owner.currentUnit.currentPoint);
        yield return null;
        owner.ChangeState<StartPlayerTurnState>();
    }
    public void ShowUnitStatus()
    {
        //if (owner.currentTile.content != null)
        //{
        //    if (owner.currentTile.content.GetComponent<Unit>() != null)
        //    {
        //        if (!owner.unitStatusUI.gameObject.activeSelf)
        //        {
        //            owner.unitStatusUI.gameObject.SetActive(true);
        //        }

        //        owner.unitStatusUI.UpdateUnit(owner.currentTile.content.GetComponent<Unit>());
        //    }
        //}

        //else
        //{
        //    if (owner.unitStatusUI.gameObject.activeSelf)
        //    {
        //        owner.unitStatusUI.gameObject.SetActive(false);
        //    }
        //}
    }

    //Version anterior
    //protected override void OnMove(object sender, InfoEventArgs<Point> e)
    //{
    //    SelectTile(e.info + pos);

    //    ShowUnitStatus();
    //}

    //protected override void  OnMouseSelectEvent(object sender, InfoEventArgs<Point> e)
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if(Physics.Raycast(ray, out hit, 100))
    //    {
    //        var a = hit.transform.gameObject;
    //        var t = a.GetComponent<Tile>();
    //        if (t != null)
    //        {
    //            SelectTile(e.info + t.pos);
    //            ShowUnitStatus();
    //        }

    //    }

    //}

    //protected override void OnMouseConfirm(object sender, InfoEventArgs<KeyCode> e)
    //{
    //    GameObject content = owner.currentTile.content;
    //    if (content != null)
    //    {
    //        if (content.GetComponent<Unit>() != null)
    //        {
    //            if (content.GetComponent<PlayerUnit>() != null)
    //            {
    //                Unit unitInTile = content.GetComponent<Unit>();

    //                if (!unitInTile.turnEnded)
    //                {
    //                    owner.currentUnit = content.GetComponent<PlayerUnit>();
    //                    owner.ChangeState<SelectActionState>();
    //                }
    //            }

    //        }

    //    }
    //}

    //protected override void OnFire(object sender, InfoEventArgs<KeyCode> e)
    //{
    //    GameObject content = owner.currentTile.content;
    //    if (content != null)
    //    {
    //        if (content.GetComponent<Unit>() != null)
    //        {
    //            if(content.GetComponent<PlayerUnit>() != null)
    //            {
    //                Unit unitInTile = content.GetComponent<Unit>();

    //                if (!unitInTile.turnEnded)
    //                {
    //                    owner.currentUnit = content.GetComponent<PlayerUnit>();
    //                    owner.ChangeState<SelectActionState>();
    //                }
    //            }

    //        }

    //    }
    //}

    //IEnumerator CanAct()
    //{
    //    if(unitsWithActions.Count == 0)
    //    {
    //        yield return null;
    //        owner.ChangeState<StartEnemyTurnState>();
    //    }
    //}


}
