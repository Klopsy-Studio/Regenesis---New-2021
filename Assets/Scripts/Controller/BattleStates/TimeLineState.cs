using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLineState : BattleState
{
    [SerializeField] PlayerUnit selectedUnit;

    TimelineElements currentElement;

    float timer = 2f;
    bool timerCheck;
    bool pause = false;
    public List<Tile> selectTiles;
    public override void Enter()
    {
        base.Enter();
        if(currentElement != null)
        {
            if (currentElement.elementEnabled)
            {
                owner.timelineUI.ShowTimelineIcon(currentElement);
                currentElement = null;
            }
        }
        owner.timelineUI.HideIconActing();
        owner.turnStatusUI.DeactivateTurn();
        owner.isTimeLineActive = true;
    }

    
    private void Update()
    {

        if (selectTiles != null && !owner.timelineUI.CheckMouse())
        {
            Debug.Log("Bruh");
            board.DeSelectDefaultTiles(selectTiles);
            selectTiles.Clear();
        }

        if (owner.isTimeLineActive && !owner.timelineUI.CheckMouse() && !owner.pauseTimeline)
        {

            if (selectedUnit != null)
            {
                selectedUnit.status.ChangeToSmall();
                selectedUnit = null;
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
                    AudioManager.instance.Play("TurnTransition");

                    currentElement = t;
                    owner.timelineUI.ShowIconActing(t);
                    owner.timelineUI.HideTimelineIcon(t);
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

                    if(t is PlayerUnitDeath r)
                    {
                        SelectTile(r.unit.currentPoint);
                        owner.currentUnit = r.unit;
                        owner.ChangeState<PlayerUnitDeathState>();
                        break;
                    }

                    if(t is MonsterEvent m)
                    {
                        SelectTile(m.controller.currentEnemy.tile.pos);
                        owner.currentMonsterEvent = m;
                        owner.ChangeState<MonsterEventState>();
                    }

                    if(t is HunterEvent h)
                    {
                        SelectTile(h.unit.currentPoint);
                        owner.currentHunterEvent = h;
                        owner.ChangeState<HunterEventState>();
                    }
                }
               
            }

            
        }

        else
        {
            if (owner.timelineUI.selectedIcon != null && owner.isTimeLineActive)
            {
                if (owner.timelineUI.selectedIcon.element.GetComponent<Unit>() != null)
                {
                    if (owner.timelineUI.selectedIcon.element.GetComponent<PlayerUnit>() != null)
                    {
                        if(selectedUnit == null)
                        {
                            selectedUnit = owner.timelineUI.selectedIcon.element.GetComponent<PlayerUnit>();
                            selectedUnit.status.ChangeToBig();
                        }
                        else
                        {
                            if(selectedUnit != owner.timelineUI.selectedIcon.element.GetComponent<PlayerUnit>())
                            {
                                selectedUnit.status.ChangeToSmall();
                                selectedUnit = owner.timelineUI.selectedIcon.element.GetComponent<PlayerUnit>();
                                selectedUnit.status.ChangeToBig();
                            }
                        }
                    }

                    SelectTile(owner.timelineUI.selectedIcon.element.GetComponent<Unit>().tile.pos);
                }

                if(owner.timelineUI.selectedIcon.element.GetComponent<PlayerUnitDeath>()!= null)
                {
                    selectedUnit = owner.timelineUI.selectedIcon.element.GetComponent<PlayerUnitDeath>().unit;
                    selectedUnit.status.ChangeToBig();

                    SelectTile(selectedUnit.currentPoint);

                }

                if(owner.timelineUI.selectedIcon.element.timelineTypes == TimeLineTypes.EnemyEvent)
                {
                    selectTiles = owner.timelineUI.selectedIcon.element.GetComponent<MonsterEvent>().GetEventTiles();
                    board.SelectAttackTiles(selectTiles);
                }

                if(owner.timelineUI.selectedIcon.element.timelineTypes == TimeLineTypes.HunterEvent)
                {
                    HunterEvent h = owner.timelineUI.selectedIcon.element.GetComponent<HunterEvent>();

                    if(h.target != null)
                    {
                        if (h.target.GetComponent<EnemyUnit>() != null)
                        {
                            EnemyUnit e = h.target.GetComponent<EnemyUnit>();
                            selectTiles = e.GiveMonsterSpace(board);
                            SelectTile(e.currentPoint);
                            board.SelectAttackTiles(selectTiles);
                        }
                        else
                        {
                            Point p = new Point((int)h.target.transform.position.x, (int)h.target.transform.position.z);
                            selectTiles.Add(board.GetTile(p));
                            board.SelectAttackTiles(selectTiles);
                            SelectTile(p);
                        }
                    }
                }

                owner.timelineUI.selectedIcon.Grow();
            }


        }

    }

    protected override void OnMouseSelectEvent(object sender, InfoEventArgs<Point> e)
    {
        
    }


}
