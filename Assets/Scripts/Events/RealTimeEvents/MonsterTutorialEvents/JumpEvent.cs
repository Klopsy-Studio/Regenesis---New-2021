using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEvent : MonsterEvent
{
    public PlayerUnit target;
    [SerializeField] Tile targetTile;
    public Vector3 originalPos;

    List<Tile> tiles = new List<Tile>();
    // Start is called before the first frame update
    public override IEnumerator Event()
    {
        acting = true;
        controller.currentEnemy.UpdateEnemyUnitSprite();
        BattleController battleController = controller.battleController;
        List<Tile> tiles = GetEventTiles();
        battleController.SelectTile(controller.currentEnemy.tile.pos);

        controller.monsterAnimations.SetBool("hide", true);

        //Tile animation:
        yield return new WaitForSeconds(1f);

        List<PlayerUnit> targets = new List<PlayerUnit>();

        foreach (Tile t in tiles)
        {
            if (t.content != null)
            {
                if (t.content.GetComponent<PlayerUnit>() != null)
                {
                    if (!t.content.GetComponent<PlayerUnit>().isNearDeath)
                    {
                        targets.Add(t.content.GetComponent<PlayerUnit>());
                    }
                }
            }
        }

        battleController.SelectTile(target.tile.pos);
        controller.gameObject.transform.position = target.transform.position;
        battleController.board.SelectAttackTiles(tiles);

        ActionEffect.instance.Play(6, 0.5f, 0.01f, 0.05f);
        controller.monsterAnimations.SetBool("appear", true);

        yield return new WaitForSeconds(0.1f);

        if (targets.Count > 0)
        {
            foreach (PlayerUnit p in targets)
            {
                //Replace with actual ability
                p.ReceiveDamage(20, false);
            }
        }

        controller.monsterAnimations.SetBool("hide", false);

        controller.monsterAnimations.SetBool("appear", false);



        while (ActionEffect.instance.play || ActionEffect.instance.recovery)
        {
            yield return null;
        }

        controller.monsterAnimations.SetBool("hide", true);
        yield return new WaitForSeconds(0.5f);
        controller.monsterAnimations.SetBool("appear", true);

        controller.gameObject.transform.position = originalPos;


        yield return new WaitForSeconds(0.5f);

        controller.monsterAnimations.SetBool("hide", false);
        controller.monsterAnimations.SetBool("appear", false);

        battleController.board.DeSelectTiles(tiles);

        acting = false;
    }

    public override List<Tile> GetEventTiles()
    {
        if(tiles.Count == 0)
        {
            tiles = new List<Tile>();
            AbilityRange range = rangeDisplay[0].GetOrCreateRange(rangeDisplay[0].range, target.gameObject);
            range.unit = target;
            tiles = range.GetTilesInRange(controller.battleController.board);
        }

        return tiles;


    }

    public override void AssignVariables()
    {
        target = controller.target;
        originalPos = controller.transform.position;
        GetEventTiles();
    }
}
