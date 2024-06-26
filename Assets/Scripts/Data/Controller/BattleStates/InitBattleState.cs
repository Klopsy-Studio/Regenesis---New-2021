using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        owner.currentLevelManager = board.Load(levelData).GetComponent<LevelManager>();
        owner.canToggleTimeline = false;
        SpawnUnits();
        yield return null;
        //owner.ChangeState<StartPlayerTurnState>();
        SelectTile(new Point(1, 5));
        owner.timelineUI.gameObject.SetActive(false);

        //Ambient foreach level
        if(levelData.ambientPrefab != null)
        {
            Instantiate(levelData.ambientPrefab);
        }
        //AudioManager.instance.Play("Music");
        //owner.unitStatusUI.gameObject.SetActive(false);

        owner.cinemachineCamera.m_Lens.NearClipPlane = -5f;
        owner.lootSystem.dropContainer = levelData.dropContainer;
        owner.ChangeState<MonsterRoarState>();
    }

    void SpawnUnits()
    {
        System.Type[] components = new System.Type[] { typeof(WalkMovement) };

        for (int i = 0; i < GameManager.instance.unitProfilesList.Length; i++)
        {
            GameObject instance = Instantiate(GameManager.instance.unitsPrefab);
            instance.name = GameManager.instance.unitProfilesList[i].name;
            PlayerUnit player = instance.GetComponent<PlayerUnit>();
            AssignUnitData(GameManager.instance.unitProfilesList[i], player);

            player.profile = GameManager.instance.unitProfilesList[i];
            Point p = levelData.playerSpawnPoints.ToArray()[i];

            player.animations.SetCharacter(GameManager.instance.unitProfilesList[i].characterIndex);
            Unit unit = instance.GetComponent<Unit>();
            unit.controller = owner;
            unit.Place(board.GetTile(p));
            unit.Match();


            Movement m = instance.AddComponent(components[0]) as Movement;
            m.jumpHeight = 1;

            //Party Icon
            UnitPartyIcon iconParty = Instantiate(owner.partyIconPrefab, owner.partyIconParent.transform).GetComponent<UnitPartyIcon>();

            
            iconParty.AssignPartyIcon(player);
            iconParty.timeline = GetComponent<TimeLineState>();
            iconParty.owner = owner;

            unitsInGame.Add(unit);
            owner.playerUnits.Add(unit);
        }

        //Disabling it for new mini Status
        //owner.unitStatusUI.SpawnUnitStatus(playerUnits);

        for (int i = 0; i < levelData.enemySpawnPoints.Count; i++)
        {

            if (levelData.enemyInLevel == null) break;
            GameObject instance = Instantiate(levelData.enemyInLevel) as GameObject;
            Point p = levelData.enemySpawnPoints.ToArray()[i];

            Unit unit = instance.GetComponent<Unit>();
            unit.controller = owner;
            unit.Place(board.GetTile(p));

            unit.currentPoint = p;
            unit.Match();

            unit.GetComponent<EnemyUnit>().UpdateMonsterSpace(board);

            MonsterMovement m = instance.GetComponent<MonsterMovement>();
            m.range = 10;
            m.jumpHeight = 1;

            unitsInGame.Add(unit);
            owner.enemyUnits.Add(unit);
        }

       
        foreach (var unit in unitsInGame)
        {
            owner.timelineElements.Add(unit);
        }


        //Removing the enviroment event for now
        //owner.timelineElements.Add(owner.environmentEvent);

        if (owner.levelData.enviromentEvent != null)
        {
            owner.environmentEvent = Instantiate(levelData.enviromentEvent, transform).GetComponent<RealTimeEvents>();
            owner.environmentEvent.battleController = owner;
            owner.environmentEvent.InitialSettings();
            owner.timelineElements.Add(owner.environmentEvent);
        }

    }

    public void AssignUnitData(UnitProfile data, PlayerUnit unit)
    {
        unit.timelineIconIndex = data.characterIconIndex;
        unit.unitPortrait = data.unitPortrait;
        unit.weapon = data.unitWeapon;
        unit.unitName = data.unitName;

        unit.timelineIcon = data.unitTimelineIcon;

    }
}
