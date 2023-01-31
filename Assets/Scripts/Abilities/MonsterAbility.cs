using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfTarget
{
    SingleTarget, MultipleTarget
}

[System.Serializable]
[CreateAssetMenu(menuName = "Ability/New Monster Ability")]

public class MonsterAbility : ScriptableObject
{
    public EffectType typeOfAbility;
    [Header("Attack Animation Trigger")]
    public string attackTrigger;
    [Header("Attack Range")]
    [SerializeField] List<RangeData> attackRange;
    [SerializeField] List<RangeData> attackRangeShow;
    [Header("Damage")]
    //Variables relacionado con daño
    public float initialDamage;
    float finalDamage;


    [Range(0.1f, 1f)]
    [SerializeField] public float abilityModifier; //CAMBIAR ESTA VARIABLE A PUBLICA Y HACER QUE SEA UN SLIDE ENTRE 0 A 1 

    [Header("Heal")]
    //Si la habilidad es de curación, se utilizan estas variables
    [SerializeField]  public float initialHeal;
    float finalHeal;

    [Header("Buff")]
    //Si la habilidad es un bufo, se usará esto
    [SerializeField]  public float initialBuff;

    [Header("Debuff")]
    //Si la habilidad es de debuffo, se usará esto
    [SerializeField] public float initialDebuff;


    [Header("Effects")]
    public List<Effect> inAbilityEffects;
    public List<Effect> postAbilityEffect;


    public TypeOfTarget targetType;
    public bool CheckIfAttackIsValid(MonsterController monster)
    {
        List<Tile> attackTiles = new List<Tile>();

        foreach (RangeData r in attackRange)
        {
            switch (r.range)
            {
                case TypeOfAbilityRange.LineAbility:
                    LineAbilityRange lineRange = monster.GetRange<LineAbilityRange>();
                    lineRange.AssignVariables(r);
                    if (CheckForUnits(lineRange.GetTilesInRange(monster.battleController.board), monster))
                    {
                        return true;
                    }
                    break;
                case TypeOfAbilityRange.Side:
                    SideAbilityRange sideRange = monster.GetRange<SideAbilityRange>();
                    sideRange.AssignVariables(r);
                    if (CheckForUnits(sideRange.GetTilesInRange(monster.battleController.board), monster))
                    {
                        return true;
                    }
                    break;
                case TypeOfAbilityRange.Cross:
                    CrossAbilityRange crossRange = monster.GetRange<CrossAbilityRange>();
                    crossRange.AssignVariables(r);
                    if (CheckForUnits(crossRange.GetTilesInRange(monster.battleController.board), monster))
                    {
                        return true;
                    }
                    break;
                case TypeOfAbilityRange.AlternateSide:
                    AlternateSideRange alternateSide = monster.GetRange<AlternateSideRange>();
                    alternateSide.AssignVariables(r);
                    if (CheckForUnits(alternateSide.GetTilesInRange(monster.battleController.board), monster))
                    {
                        return true;
                    }
                    break;

                default:
                    break;
            }
        }

        return false;
    }

    public List<Tile> GetAttackTiles(MonsterController monster)
    {
        List<Tile> retValue = new List<Tile>();
        foreach (RangeData r in attackRange)
        {
            switch (r.range)
            {
                case TypeOfAbilityRange.LineAbility:
                    LineAbilityRange lineRange = monster.GetRange<LineAbilityRange>();
                    lineRange.AssignVariables(r);
                    List<Tile> lineTiles = lineRange.GetTilesInRange(monster.battleController.board);
                    AddTilesToList(retValue, lineTiles);
                    break;
                case TypeOfAbilityRange.Side:
                    SideAbilityRange sideRange = monster.GetRange<SideAbilityRange>();
                    sideRange.AssignVariables(r);
                    List<Tile> sideTiles = sideRange.GetTilesInRange(monster.battleController.board);
                    AddTilesToList(retValue, sideTiles);
                    break;

                case TypeOfAbilityRange.Cross:
                    CrossAbilityRange crossRange = monster.GetRange<CrossAbilityRange>();
                    crossRange.AssignVariables(r);
                    List<Tile> crossTiles = crossRange.GetTilesInRange(monster.battleController.board);
                    AddTilesToList(retValue, crossTiles);
                    break;

                case TypeOfAbilityRange.AlternateSide:
                    AlternateSideRange altSideRange = monster.GetRange<AlternateSideRange>();
                    altSideRange.AssignVariables(r);
                    List<Tile> altSideTiles = altSideRange.GetTilesInRange(monster.battleController.board);
                    AddTilesToList(retValue, altSideTiles);
                    break;
                default:
                    break;
            }
        }

        return retValue;
    }
    public bool CheckForUnits(List<Tile> tileList, MonsterController placeholder)
    {
        foreach(Tile t in tileList)
        {
            if(t.content != null)
            {
                if(t.content.GetComponent<PlayerUnit>() != null)
                {
                    if (!t.content.GetComponent<PlayerUnit>().isNearDeath)
                    {
                        placeholder.target = t.content.GetComponent<PlayerUnit>();
                        return true;

                    }
                }
            }
        }

        return false;
    }

    public List<Tile> ShowAttackRange(Directions dir, MonsterController monster)
    {
        List<Tile> retValue = new List<Tile>();
        foreach (RangeData r in attackRange)
        {
            switch (r.range)
            {
                case TypeOfAbilityRange.LineAbility:
                    LineAbilityRange lineRange = monster.GetRange<LineAbilityRange>();
                    lineRange.AssignVariables(r);
                    lineRange.lineDir = dir;
                    List<Tile> lineTiles = lineRange.GetTilesInRange(monster.battleController.board);
                    AddTilesToList(retValue, lineTiles);
                    break;
                case TypeOfAbilityRange.Side:
                    SideAbilityRange sideRange = monster.GetRange<SideAbilityRange>();
                    sideRange.AssignVariables(r);
                    sideRange.sideDir = dir;
                    List<Tile> sideTiles = sideRange.GetTilesInRange(monster.battleController.board);
                    AddTilesToList(retValue, sideTiles);
                    break;

                case TypeOfAbilityRange.Cross:
                    CrossAbilityRange crossRange = monster.GetRange<CrossAbilityRange>();
                    crossRange.AssignVariables(r);
                    List<Tile> crossTiles = crossRange.GetTilesInRange(monster.battleController.board);
                    AddTilesToList(retValue, crossTiles);
                    break;

                case TypeOfAbilityRange.AlternateSide:
                    AlternateSideRange altSideRange = monster.GetRange<AlternateSideRange>();
                    altSideRange.AssignVariables(r);
                    altSideRange.alternateSideDir = dir;
                    List<Tile> altSideTiles = altSideRange.GetTilesInRange(monster.battleController.board);
                    AddTilesToList(retValue, altSideTiles);
                    break;
                default:
                    break;
            }
        }

        return retValue;
    }
    public List<PlayerUnit> ReturnPossibleTargets(MonsterController monster)
    {
        List<Tile> tiles = GetAttackTiles(monster);
        List<PlayerUnit> units = new List<PlayerUnit>();

        foreach(Tile t in tiles)
        {
            if(t.content != null)
            {
                if(t.content.GetComponent<PlayerUnit>() != null)
                {
                    units.Add(t.content.GetComponent<PlayerUnit>());
                }
            }
        }

        return units;
    }
    public void AddTilesToList(List<Tile> tileHolder, List<Tile> newTiles)
    {
        foreach (Tile t in newTiles)
        {
            if (!tileHolder.Contains(t))
            {
                tileHolder.Add(t);
            }
        }
    }


    public void UseAbility(PlayerUnit target, EnemyUnit enemy, BattleController controller)
    {
        CalculateDmg(enemy, target);
        target.ReceiveDamage(finalDamage);
    }

    void CalculateDmg(EnemyUnit enemy, PlayerUnit target)
    {
        float criticalDmg = 1f;
        if (Random.value * 100 <= enemy.criticalPercentage) criticalDmg = 1.5f;
        float elementDmg = ElementsEffectiveness.GetEffectiveness(enemy.attackElement, target.defenseElement);

        finalDamage = (((enemy.power * criticalDmg) + (enemy.power * enemy.elementPower) * elementDmg) * abilityModifier) - target.defense;
    }
}
