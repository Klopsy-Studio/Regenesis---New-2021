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
        List<Tile> tiles = range.GetTilesInRange(controller.board);
        List<Unit> units = new List<Unit>();

        foreach(Tile t in tiles)
        {
            t.SetSmokeBomb();
            if(t.content != null)
            {
                if(t.content.GetComponent<Unit>() != null)
                {
                    if (!units.Contains(t.content.GetComponent<Unit>()))
                    {
                        units.Add(t.content.GetComponent<Unit>());
                    }
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
