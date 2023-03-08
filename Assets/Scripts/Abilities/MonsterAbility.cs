using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfTarget
{
    SingleTarget, MultipleTarget, RandomSingleTarget
}

[System.Serializable]
[CreateAssetMenu(menuName = "Ability/New Monster Ability")]

public class MonsterAbility : ScriptableObject
{
    public EffectType typeOfAbility;
    [Header("Attack Animation Trigger")]
    public string attackTrigger;
    [Header("Attack Range")]
    public List<RangeData> attackRange;
    [SerializeField] List<RangeData> removeRange;
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
            AbilityRange range = r.GetOrCreateRange(r.range, monster.gameObject);
            range.unit = monster.currentEnemy;

            if (CheckForUnits(range.GetTilesInRange(monster.battleController.board), monster))
            {
                return true;
            }

            //switch (r.range)
            //{
            //    case TypeOfAbilityRange.LineAbility:
            //        LineAbilityRange lineRange = monster.GetRange<LineAbilityRange>();
            //        lineRange.AssignVariables(r);
            //        if (CheckForUnits(lineRange.GetTilesInRange(monster.battleController.board), monster))
            //        {
            //            return true;
            //        }
            //        break;
            //    case TypeOfAbilityRange.Side:
            //        SideAbilityRange sideRange = monster.GetRange<SideAbilityRange>();
            //        sideRange.AssignVariables(r);
            //        if (CheckForUnits(sideRange.GetTilesInRange(monster.battleController.board), monster))
            //        {
            //            return true;
            //        }
            //        break;
            //    case TypeOfAbilityRange.Cross:
            //        CrossAbilityRange crossRange = monster.GetRange<CrossAbilityRange>();
            //        crossRange.AssignVariables(r);
            //        if (CheckForUnits(crossRange.GetTilesInRange(monster.battleController.board), monster))
            //        {
            //            return true;
            //        }
            //        break;
            //    case TypeOfAbilityRange.AlternateSide:
            //        AlternateSideRange alternateSide = monster.GetRange<AlternateSideRange>();
            //        alternateSide.AssignVariables(r);
            //        if (CheckForUnits(alternateSide.GetTilesInRange(monster.battleController.board), monster))
            //        {
            //            return true;
            //        }
            //        break;

            //    default:
            //        break;
            //}
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
                    lineRange.unit = monster.currentEnemy;
                    List<Tile> lineTiles = lineRange.GetTilesInRange(monster.battleController.board);
                    AddTilesToList(retValue, lineTiles);
                    break;
                case TypeOfAbilityRange.Side:
                    SideAbilityRange sideRange = monster.GetRange<SideAbilityRange>();
                    sideRange.AssignVariables(r);
                    sideRange.unit = monster.currentEnemy;
                    List<Tile> sideTiles = sideRange.GetTilesInRange(monster.battleController.board);
                    AddTilesToList(retValue, sideTiles);
                    break;

                case TypeOfAbilityRange.Cross:
                    CrossAbilityRange crossRange = monster.GetRange<CrossAbilityRange>();
                    crossRange.AssignVariables(r);
                    crossRange.unit = monster.currentEnemy;
                    List<Tile> crossTiles = crossRange.GetTilesInRange(monster.battleController.board);
                    AddTilesToList(retValue, crossTiles);
                    break;

                case TypeOfAbilityRange.AlternateSide:
                    AlternateSideRange altSideRange = monster.GetRange<AlternateSideRange>();
                    altSideRange.AssignVariables(r);
                    altSideRange.unit = monster.currentEnemy;

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

        if (removeRange.Count > 0 && removeRange != null)
        {
            foreach (RangeData r in attackRange)
            {
                switch (r.range)
                {
                    case TypeOfAbilityRange.LineAbility:
                        LineAbilityRange lineRange = monster.GetRange<LineAbilityRange>();
                        lineRange.AssignVariables(r);
                        lineRange.lineDir = dir;
                        List<Tile> lineTiles = lineRange.GetTilesInRange(monster.battleController.board);
                        
                        foreach(Tile t in lineTiles)
                        {
                            if (retValue.Contains(t))
                            {
                                retValue.Remove(t);
                            }
                        }
                        break;
                    case TypeOfAbilityRange.Side:
                        SideAbilityRange sideRange = monster.GetRange<SideAbilityRange>();
                        sideRange.AssignVariables(r);
                        sideRange.sideDir = dir;
                        List<Tile> sideTiles = sideRange.GetTilesInRange(monster.battleController.board);

                        foreach (Tile t in sideTiles)
                        {
                            if (retValue.Contains(t))
                            {
                                retValue.Remove(t);
                            }
                        }
                        break;

                    case TypeOfAbilityRange.Cross:
                        CrossAbilityRange crossRange = monster.GetRange<CrossAbilityRange>();
                        crossRange.AssignVariables(r);
                        List<Tile> crossTiles = crossRange.GetTilesInRange(monster.battleController.board);
          

                        foreach (Tile t in crossTiles)
                        {
                            if (retValue.Contains(t))
                            {
                                retValue.Remove(t);
                            }
                        }
                        break;

                    case TypeOfAbilityRange.AlternateSide:
                        AlternateSideRange altSideRange = monster.GetRange<AlternateSideRange>();
                        altSideRange.AssignVariables(r);
                        altSideRange.alternateSideDir = dir;
                        List<Tile> altSideTiles = altSideRange.GetTilesInRange(monster.battleController.board);

                        foreach (Tile t in altSideTiles)
                        {
                            if (retValue.Contains(t))
                            {
                                retValue.Remove(t);
                            }
                        }
                        break;
                    default:
                        break;
                }
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
        //It's always true since we don't show critcial hits from monsters to units
        CalculateDmg(enemy, target);
        target.ReceiveDamage((int)finalDamage, true);
    }

    void CalculateDmg(EnemyUnit enemy, PlayerUnit target)
    {
        float criticalDmg = 1f;
        if (Random.value * 100 <= enemy.criticalPercentage) criticalDmg = 1.5f;
        float elementDmg = ElementsEffectiveness.GetEffectiveness(enemy.attackElement, target.defenseElement);
        int finalPower = enemy.power;

        if(target.debuffModifiers != null)
        {
            if(target.debuffModifiers.Count > 0)
            {
                List<Modifier> trash = new List<Modifier>();
                foreach (Modifier m in target.debuffModifiers)
                {
                    if (m.modifierType == TypeOfModifier.Damage)
                    {
                        finalPower += m.damageIncrease;
                        trash.Add(m);
                    }

                }

                if (trash.Count > 0)
                {
                    foreach (Modifier m in trash)
                    {
                        target.RemoveDebuff(m);
                    }
                }

                target.marked = false;
            }
            
        }

        if (target.buffModifiers != null)
        {
            if (target.buffModifiers.Count > 0)
            {
                List<Modifier> trash = new List<Modifier>();

                foreach (Modifier d in target.buffModifiers)
                {
                    if (d.modifierType == TypeOfModifier.Defense)
                    {
                        abilityModifier -= d.damageReduction;

                        if (d.SpendModifier())
                        {
                            trash.Add(d);
                        }

                        break;
                    }
                }

                if (trash.Count > 0)
                {
                    foreach (Modifier d in trash)
                    {
                        target.RemoveBuff(d);
                    }
                }
            }
        }
        finalDamage = (((finalPower * criticalDmg) + (enemy.power * enemy.elementPower) * elementDmg) * abilityModifier)-target.defense;

        Debug.Log("Target: " + target.unitName + " Damage dealt: " + finalDamage);
    }
}
