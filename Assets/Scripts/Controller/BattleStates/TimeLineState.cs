using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLineState : BattleState
{
    [SerializeField] PlayerUnit selectedUnit;

    public TimelineElements currentElement;
    float timer = 2f;
    bool timerCheck;
    bool pause = false;
    public List<Tile> selectTiles;
    public override void Enter()
    {
        base.Enter();
        //if (currentElement != null)
        //{
        //    if (currentElement.elementEnabled)
        //    {
        //        owner.timelineUI.ShowTimelineIcon(currentElement);
        //        currentElement = null;
        //    }
        //}
        //owner.timelineUI.HideIconActing();
        owner.resumeTimelineButton.onUp.Invoke();
        owner.EnableResumeTimelineButton();
        owner.canToggleTimeline = true;
        owner.turnStatusUI.DeactivateTurn();
        owner.isTimeLineActive = true;
    }

    public void CheckIcon()
    {
        if (owner.timelineUI.selectedIcon.element.GetComponent<Unit>() != null)
        {
            if (owner.timelineUI.selectedIcon.element.GetComponent<PlayerUnit>() != null)
            {
                owner.ZoomIn();

                if (selectedUnit == null)
                {
                    selectedUnit = owner.timelineUI.selectedIcon.element.GetComponent<PlayerUnit>();
                    Debug.Log("Setting Unit");
                    //selectedUnit.status.ChangeToBig();
                }
                else
                {
                    if (selectedUnit != owner.timelineUI.selectedIcon.element.GetComponent<PlayerUnit>())
                    {
                        //selectedUnit.status.ChangeToSmall();
                        selectedUnit = owner.timelineUI.selectedIcon.element.GetComponent<PlayerUnit>();
                        //selectedUnit.status.ChangeToBig();
                        Debug.Log("Setting Unit");
                    }
                }
                owner.SelectTile(selectedUnit.tile.pos);
                owner.miniStatus.SetStatus(selectedUnit);

            }

            if (owner.timelineUI.selectedIcon.element.GetComponent<EnemyUnit>() != null)
            {
                Debug.Log("enemy");
                owner.timelineUI.selectedIcon.element.GetComponent<EnemyUnit>();
                owner.miniStatus.SetStatus(owner.timelineUI.selectedIcon.element.GetComponent<EnemyUnit>());
            }

            SelectTile(owner.timelineUI.selectedIcon.element.GetComponent<Unit>().tile.pos);
        }

        if (owner.timelineUI.selectedIcon.element.GetComponent<PlayerUnitDeath>() != null)
        {
            selectedUnit = owner.timelineUI.selectedIcon.element.GetComponent<PlayerUnitDeath>().unit;
            SelectTile(selectedUnit.currentPoint);
        }

        if (owner.timelineUI.selectedIcon.element.timelineTypes == TimeLineTypes.Items)
        {
            owner.SelectTile(owner.timelineUI.selectedIcon.element.GetComponent<BombTimeline>().currentPoint);
            owner.miniStatus.SetStatus(owner.timelineUI.selectedIcon.element);
        }
    }
    protected override void OnMouseConfirm(object sender, InfoEventArgs<KeyCode> e)
    {
        if (owner.timelineUI.selectedIcon != null && owner.isTimeLineActive)
        {
            if (owner.timelineUI.selectedIcon.mouseOver)
            {
                if(owner.currentSelectedIcon != null)
                {
                    if (owner.currentSelectedIcon != owner.timelineUI.selectedIcon)
                    {
                        owner.currentSelectedIcon.Return();
                        owner.timelineUI.selectedIcon.selected = true;
                        owner.currentSelectedIcon = owner.timelineUI.selectedIcon;
                        CheckIcon();             
                    }
                    else
                    {
                        owner.currentSelectedIcon.Return();
                        owner.currentSelectedIcon = null;
                        CleanPause();
                    }
                }
                else
                {
                    owner.currentSelectedIcon = owner.timelineUI.selectedIcon;
                    CheckIcon();
                }
            }
            else
            {
                CleanPause();
                owner.timelineUI.selectedIcon.selected = false;

                owner.timelineUI.selectedIcon.Return();
                owner.timelineUI.selectedIcon = null;
            }
        }
       
    }

    public void CleanPause()
    {
        if (selectTiles != null)
        {
            board.DeSelectDefaultTiles(selectTiles);
            selectTiles.Clear();
        }
        
        if (owner.zoomed)
        {
            owner.ZoomOut();
        }
    }
    private void Update()
    {
        if (!owner.battleEnded)
        {
            if (owner.isTimeLineActive && !owner.pauseTimeline)
            {
                if (selectedUnit != null)
                {
                    owner.miniStatus.DeactivateStatus();
                    selectedUnit = null;
                }

                if(owner.timelineUI.selectedIcon != null)
                {
                    owner.timelineUI.selectedIcon.Return();
                    owner.timelineUI.selectedIcon.selected = false;

                }
                //Timeline del evento real
                //owner.realTimeEvent.UpdateTimeLine();

                foreach (var t in owner.timelineElements)
                {
                    if (t == null) { return; }
                    //if (owner.IsInMenu()) continue;

                    bool isTimeline = t.UpdateTimeLine();

                    if (isTimeline)
                    {
                        owner.canToggleTimeline = false;
                        owner.pauseTimelineButton.onUp.Invoke();
                        owner.DisableResumeTimelineButton();

                        AudioManager.instance.Play("TurnTransition");

                        currentElement = t;
                        owner.timelineUI.ShowIconActing(t);
                        owner.timelineUI.HideTimelineIcon(t);
                        owner.ZoomOut();
                        if (t is PlayerUnit p)
                        {

                            owner.currentUnit = p;
                            owner.currentUnit.playerUI.ResetActionPoints();
                            owner.ChangeState<SelectUnitState>();
                            break;
                        }

                        if (t is EnemyUnit e)
                        {
                            owner.currentEnemyUnit = e;
                            SelectTile(owner.currentEnemyUnit.tile.pos);
                            //owner.currentEnemyController = e.GetComponent<EnemyController>();
                            //owner.currentEnemyController.battleController = owner;

                            owner.monsterController = e.GetComponent<MonsterController>();
                            owner.monsterController.battleController = owner;
                            owner.ChangeState<StartEnemyTurnState>();
                            break;
                        }

                        if (t is RealTimeEvents)
                        {
                            owner.ChangeState<EventActiveState>();
                            break;
                        }

                        if (t is ItemElements w)
                        {
                            owner.currentItem = w;
                            owner.turnStatusUI.ActivateTurn("Item");
                            SelectTile(owner.currentItem.tile.pos);
                            owner.ChangeState<ItemActiveState>();
                            break;
                        }

                        if (t is PlayerUnitDeath r)
                        {
                            SelectTile(r.unit.currentPoint);
                            owner.currentUnit = r.unit;
                            owner.ChangeState<PlayerUnitDeathState>();
                            break;
                        }

                        if (t is MonsterEvent m)
                        {
                            SelectTile(m.controller.currentEnemy.tile.pos);
                            owner.currentMonsterEvent = m;
                            owner.ChangeState<MonsterEventState>();
                        }

                        if (t is HunterEvent h)
                        {
                            SelectTile(h.unit.currentPoint);
                            owner.currentHunterEvent = h;
                            owner.ChangeState<HunterEventState>();
                        }
                    }

                }
            }
        }
    }

    


}
