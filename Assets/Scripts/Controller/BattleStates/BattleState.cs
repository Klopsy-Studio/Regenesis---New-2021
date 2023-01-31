using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleState : State
{
    protected BattleController owner;
    public CameraRig CameraRig { get { return owner.cameraRig; } }
    public Board board { get { return owner.board; } }
    public LevelData levelData { get { return owner.levelData; } }
    public Transform tileSelectionIndicator { get { return owner.tileSelectionIndicator; } }
    public Point pos { get { return owner.pos; } set { owner.pos = value; } }

    public List<Unit> unitsInGame { get { return owner.unitsInGame; } set { owner.unitsInGame = value; } }
    public List<Unit> playerUnits { get { return owner.playerUnits; } set { owner.playerUnits = value; } }
    public List<Unit> unitsWithActions { get { return owner.unitsWithActions; } set { owner.unitsWithActions = value; } }

    public OptionSelection ActionSelectionUI { get { return owner.actionSelectionUI; } set { owner.actionSelectionUI = value; } }

    public OptionSelection AbilitySelectionUI { get { return owner.abilitySelectionUI; } set { owner.abilitySelectionUI = value; } }

    public OptionSelection ItemSelectionUI { get { return owner.itemSelectionUI; } set { owner.itemSelectionUI = value; } }
    protected virtual void Awake()
    {
        owner = GetComponent<BattleController>();
    }

    protected override void AddListeners()
    {
        InputController.moveEvent += OnMove;
        InputController.selectEvent += OnFire;
        InputController.escapeEvent += OnEscape;
        InputController.mouseConfirmEvent += OnMouseConfirm;
        InputController.mouseSelectEvent += OnMouseSelectEvent;
        InputController.mouseCancelEvent += OnMouseCancelEvent;


        UIController.buttonClick += OnSelectAction;
        UIController.buttonCancel += OnSelectCancelEvent;
    }

    protected virtual void OnSelectAction(object sender, InfoEventArgs<int> e)
    {
       
    }

    protected virtual void OnMouseConfirm(object sender, InfoEventArgs<KeyCode> e)
    {

    }
    protected virtual void OnMouseSelectEvent(object sender, InfoEventArgs<Point> e)
    {

    }

    protected virtual void OnMove(object sender, InfoEventArgs<Point> e)
    {

    }

    protected virtual void OnFire(object sender, InfoEventArgs<KeyCode> e)
    {

    }

    protected virtual void OnEscape(object sender, InfoEventArgs<KeyCode> e)
    {

    }

    protected override void RemoveListeners()
	{
		InputController.moveEvent -= OnMove;
		InputController.selectEvent -= OnFire;
        InputController.escapeEvent -= OnEscape;
        InputController.mouseConfirmEvent -= OnMouseConfirm;
        InputController.mouseSelectEvent -= OnMouseSelectEvent;
        InputController.mouseCancelEvent -= OnMouseCancelEvent;


        UIController.buttonClick -= OnSelectAction;
        UIController.buttonCancel -= OnSelectCancelEvent;
    }

    protected virtual void OnSelectCancelEvent(object sender, InfoEventArgs<int> e)
    {

    }

    protected virtual void OnMouseCancelEvent(object sender, InfoEventArgs<KeyCode> e)
    {

    }

    protected virtual void SelectTile(Point p)
    {
        if (pos == p || !board.playableTiles.ContainsKey(p))
        {
            return;
        }

        pos = p;
        tileSelectionIndicator.localPosition = board.playableTiles[p].center;
    }

    protected virtual bool CanReachTile(Point p, List<Tile> tiles)
    {
        if(board.playableTiles[p] != null)
        {
            if (tiles.Contains(board.playableTiles[p]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public virtual T GetRange<T>() where T : AbilityRange
    {
        T target = owner.currentUnit.GetComponent<T>();

        if (target == null)
        {
            target = owner.currentUnit.gameObject.AddComponent<T>();
        }

        return target;
    }

    protected void FilterTiles(List<Tile> t)
    {
        List<Tile> tileList = new List<Tile>();

        foreach(Tile tile in t)
        {
            tileList.Add(tile);
        }

        foreach(Tile tile in tileList)
        {
            if (tile.occupied)
            {
                t.Remove(tile);
            }
        }
    }

}
