using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLineState : BattleState
{
    [SerializeField] PlayerUnit selectedUnit;

    public TimelineElements currentElement;
    public TimelineElements currentSelectedElement;

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
        owner.ChangeCurrentControls("Pause");
    }

    public void CheckIcon()
    {
        currentSelectedElement.iconTimeline.ActivateIconHightlight();
        currentSelectedElement.iconTimeline.transform.SetAsLastSibling();
        if (currentSelectedElement.TryGetComponent<Unit>(out Unit u))
        {
            if (u.TryGetComponent<PlayerUnit>(out PlayerUnit p))
            {
                owner.ZoomIn();

                if (selectedUnit == null)
                {
                    selectedUnit = p;
                    Debug.Log("Setting Unit");
                }
                else
                {
                    if (selectedUnit != p)
                    {
                        selectedUnit = p;
                        Debug.Log("Setting Unit");
                    }
                }
                owner.SelectTile(p.tile.pos);
                owner.miniStatus.SetStatus(p);
            }

            if (u.TryGetComponent<EnemyUnit>(out EnemyUnit e))
            {
                owner.miniStatus.SetStatus(e);
            }

            SelectTile(u.tile.pos);
        }

        if (currentSelectedElement.TryGetComponent<PlayerUnitDeath>(out PlayerUnitDeath playerDeath))
        {
            selectedUnit = playerDeath.unit;
            SelectTile(selectedUnit.currentPoint);
        }

        if (currentSelectedElement.timelineTypes == TimeLineTypes.Items)
        {
            owner.SelectTile(currentSelectedElement.GetComponent<BombTimeline>().currentPoint);
            owner.miniStatus.SetStatus(currentElement);
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
                    if (currentSelectedElement != null)
                    {
                        currentSelectedElement.iconTimeline.DeactivateIconHightlight();
                    }

                    if (owner.currentSelectedIcon != owner.timelineUI.selectedIcon)
                    {
                        
                        owner.currentSelectedIcon.Return();
                        owner.timelineUI.selectedIcon.selected = true;
                        owner.currentSelectedIcon = owner.timelineUI.selectedIcon;
                        currentSelectedElement = owner.currentSelectedIcon.element;
                        CheckIcon();             
                    }
                    //else
                    //{
                    //    owner.currentSelectedIcon.Return();
                    //    owner.currentSelectedIcon = null;
                    //    currentSelectedElement = null;
                    //    CleanPause();
                    //}
                }
                else
                {
                    if(currentSelectedElement != null)
                    {
                        currentSelectedElement.iconTimeline.DeactivateIconHightlight();
                    }
                    owner.currentSelectedIcon = owner.timelineUI.selectedIcon;
                    currentSelectedElement = owner.currentSelectedIcon.element;
                    CheckIcon();
                }
            }
            else
            {
                CleanPause();
                owner.timelineUI.selectedIcon.selected = false;
                owner.timelineUI.selectedIcon.Return();
                owner.timelineUI.selectedIcon = null;
                currentSelectedElement = null;
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

                    selectedUnit.iconTimeline.DeactivateIconHightlight();
                    selectedUnit = null;
                }

                if(owner.timelineUI.selectedIcon != null)
                {
                    owner.timelineUI.selectedIcon.Return();
                    owner.timelineUI.selectedIcon.DeactivateIconHightlight();
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
                        owner.DeactivateCurrentControls();
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

    protected override void OnMoveForwardEvent(object sender, InfoEventArgs<KeyCode> e)
    {
        Debug.Log("Moving Forward");
        SelectNextElement();
    }

    protected override void OnMoveBackwardsEvent(object sender, InfoEventArgs<KeyCode> e)
    {
        Debug.Log("Moving backwards");
        SelectPreviousElement();
    }

    public void SelectNextElement()
    {
        if(owner.pauseTimeline && !owner.battleEnded)
        {
            if (currentSelectedElement != null)
            {
                int timelineElementsIndex = owner.orderedTimelineSlements.IndexOf(currentSelectedElement);
                timelineElementsIndex--;

                if (timelineElementsIndex < 0)
                {
                    timelineElementsIndex = owner.orderedTimelineSlements.Count - 1;
                }

                currentSelectedElement.iconTimeline.DeactivateIconHightlight();
                currentSelectedElement = owner.orderedTimelineSlements[timelineElementsIndex];
                CheckIcon();
            }

            else
            {
                currentSelectedElement = owner.orderedTimelineSlements[owner.timelineElements.Count - 1];
                CheckIcon();
            }
        }
    }

    public void SelectPreviousElement()
    {
        if (owner.pauseTimeline && !owner.battleEnded)
        {
            if (currentSelectedElement != null)
            {
                int timelineElementsIndex = owner.orderedTimelineSlements.IndexOf(currentSelectedElement);
                timelineElementsIndex++;

                if (timelineElementsIndex >= owner.orderedTimelineSlements.Count)
                {
                    timelineElementsIndex = 0;
                }
                currentSelectedElement.iconTimeline.DeactivateIconHightlight();
                currentSelectedElement = owner.orderedTimelineSlements[timelineElementsIndex];
                CheckIcon();
            }

            else
            {
                currentSelectedElement = owner.orderedTimelineSlements[0];
                CheckIcon();
            }
            
        }
    }




}
