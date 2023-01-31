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
        board.Load(levelData);
        SpawnUnits();
        yield return null;
        //owner.ChangeState<StartPlayerTurnState>();
        SelectTile(new Point(1, 5));
        owner.timelineUI.gameObject.SetActive(false);
        owner.unitStatusUI.gameObject.SetActive(false);
        owner.ChangeState<MonsterRoarState>();
    }

    void SpawnUnits()
    {
        System.Type[] components = new System.Type[] { typeof(WalkMovement) };

        float index = 0f;
        for (int i = 0; i < GameManager.instance.unitProfilesList.Length; i++)
        {
            GameObject instance = Instantiate(GameManager.instance.unitsPrefab);
            instance.name = GameManager.instance.unitProfilesList[i].name;
            PlayerUnit player = instance.GetComponent<PlayerUnit>();
            AssignUnitData(GameManager.instance.unitProfilesList[i], player);

            player.profile = GameManager.instance.unitProfilesList[i];
            Point p = levelData.playerSpawnPoints.ToArray()[i];

            player.animations.SetCharacter(index);
            index += 0.5f;
            Unit unit = instance.GetComponent<Unit>();
            unit.controller = owner;
            unit.Place(board.GetTile(p));
            unit.Match();

            
            Movement m = instance.AddComponent(components[0]) as Movement;
            m.jumpHeight = 1;


            unitsInGame.Add(unit);
            owner.playerUnits.Add(unit);
        }

        owner.unitStatusUI.SpawnUnitStatus(playerUnits);

        for (int i = 0; i < levelData.enemySpawnPoints.Count; i++)
        {

            if (levelData.enemyInLevel == null) break;
            GameObject instance = Instantiate(levelData.enemyInLevel) as GameObject;
            Point p = levelData.enemySpawnPoints.ToArray()[i];

            Unit unit = instance.GetComponent<Unit>();
            unit.controller = owner;
            unit.Place(board.GetTile(p));

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

        owner.timelineElements.Add(owner.environmentEvent);

    }

    public void AssignUnitData(UnitProfile data, PlayerUnit unit)
    {
        unit.unitPortrait = data.unitPortrait;
        unit.fullUnitPortrait = data.unitFullPortrait;
        unit.weapon = data.unitWeapon;
        unit.unitName = data.unitName;

        unit.timelineIcon = data.unitTimelineIcon;

        unit.deathTimelineSprite = data.deathTimeline;

    }
}
