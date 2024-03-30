using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBombTimeline : MonoBehaviour
{
    public ItemRange range;
    [SerializeField] int decreaseAmmount;
    bool monsterAdded;

    public void ApplyEffect(BattleController controller)
    {
        gameObject.SetActive(true);
        StartCoroutine(ApplyEffectSequence(controller));
    }
    public IEnumerator ApplyEffectSequence(BattleController controller)
    {
        range.removeMonster = false;
        List<Tile> tiles = range.GetTilesInRange(controller.board);
        List<Unit> units = new List<Unit>();

        foreach(Tile t in tiles)
        {
            t.SetSmokeBomb();
            if(t.content != null)
            {
                if(t.content.TryGetComponent(out Unit u))
                {
                    if (!units.Contains(u))
                    {
                        units.Add(u);
                    }
                }
            }

            if (t.occupied)
            {
                if (!units.Contains(controller.enemyUnits[0]))
                {
                    units.Add(controller.enemyUnits[0]);
                }
            }
        }

        AudioManager.instance.Play("SmokeBomb");
        yield return new WaitForSeconds(0.5f);

        foreach(Unit u in units)
        {
            if (u.buffModifiers.Count > 0)
            {
                foreach (Modifier m in u.buffModifiers)
                {
                    if (m.modifierType == TypeOfModifier.Antivirus)
                    {
                        u.RemoveBuff(m);
                        break;
                    }
                }
            }
            else
            {
                u.DecreaseTimelineVelocity(decreaseAmmount);
                u.AddDebuff(new Modifier { modifierType = TypeOfModifier.TimelineSpeed, timelineSpeedReduction = decreaseAmmount });
            }
            
        }
    }
}
