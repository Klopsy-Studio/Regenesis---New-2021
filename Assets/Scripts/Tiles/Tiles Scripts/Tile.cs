using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObstacleType
{
    Null,
    RegularObstacle,
}
[System.Serializable]
public class Tile : MonoBehaviour
{
    public DataTile data;
    public int tileIndex;
    public const float stepHeight = 0.25f; 
    public Point pos;
    public int height;
    public Vector3 center { get { return new Vector3(pos.x, height * stepHeight, pos.y); } } //Center of the tile. Allow the units to center themselves

    [SerializeField] Renderer model;

    public SpriteRenderer tileSprite;

    
    public string displayName;

    

    public List<TileModifier> modifiers = new List<TileModifier>();
    

    
    public GameObject content
    {
        get
        {
            return m_content;
        }
        set
        {
            m_content = value;

            if (content == null)
            {
                OnUnitLeave();
            }
            else
            {
                OnUnitArrive();
            }
        }
    }

    public bool occupied;

    public GameObject m_content = null;

    [HideInInspector] public Tile prev; //stores the tile which was traversed to reach it
    [HideInInspector] public int distance; //numbers of tiles which have been crossed to reach the point 

    public bool selected;

    [Header("Tile Selection")]
    public Color emptyColor;
    public Color movementColor;
    public Color healColor;
    public Color attackColor;
    public Color abilityColor;
    public GameObject tileIndicator;
    public SpriteRenderer selection;

    public GameObject emptySelection;
    public GameObject selectMovementObject;
    public GameObject selectAttackObject;
    GameObject currentObject;
    [HideInInspector] public GameObject previousObject;
    public Color previousColor;
    Color currentColor;
    [Space]
    [Header("VFX")]
    [SerializeField] Animator tileVfx;
    [SerializeField] Animator eventVfx;

    private void Start()
    {
        if(tileIndicator != null)
        {
            tileIndicator.SetActive(false);
        }
        currentObject = emptySelection;
    }

    public void SetCracks()
    {
        eventVfx.SetBool("cracks", true);
    }

    public void DisableCracks()
    {
        eventVfx.SetBool("cracks", false);

    }
    public void ActivateTileSelection()
    {
        if(content != null)
        {
            if (!occupied && !content.GetComponent<Unit>())
            {
                tileIndicator.SetActive(true);
            }
            else
            {
                content.GetComponent<Unit>().tileIndicator.gameObject.SetActive(true);
            }
        }
        if (tileIndicator != null)
        {
            if (!occupied)
            {
                tileIndicator.SetActive(true);
            }
        }
    }

    public void DeactivateTileSelection()
    {
        if (tileIndicator != null)
        {
            tileIndicator.SetActive(false);
        }

        if(content!=null)
        {
            if(content.GetComponent<Unit>() != null)
            {
                content.GetComponent<Unit>().tileIndicator.SetActive(false);
            }
        }
    }
    public void Match() //Matches the values of the variables with the gameObjects transforms values
    {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void Grow()
    {
        height++;
        Match();
    }

    public void Shrink()
    {
        height--;
        Match();
    }
    
    public void SetSmokeBomb()
    {
        if(tileVfx != null)
        {
            tileVfx.SetTrigger("smoke");
        }
        else
        {
            Debug.Log("Sprite VFX animator component is missing");
        }
    }
    public void SetHealthGas()
    {
        if (tileVfx != null)
        {
            tileVfx.SetTrigger("health");
        }
        else
        {
            Debug.Log("Sprite VFX animator component is missing");
        }
    }


    public void SetRootVfx()
    {
        if (tileVfx != null)
        {
            tileVfx.SetTrigger("roots");
        }
        else
        {
            Debug.Log("Sprite VFX animator component is missing");
        }
    }
    public void Load (Point p, int h)
    {
        pos = p;
        height = 4;
        Match();
    }

    public void Load(Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }

    public virtual void OnUnitArrive()
    {
        if(content.GetComponent<Unit>() != null)
        {
            foreach(TileModifier m in modifiers)
            {
                m.Effect(content.GetComponent<Unit>());
            }

            modifiers.Clear();
        }

        
    }

    public void OnUnitArriveMonster(EnemyUnit unit)
    {
        foreach (TileModifier m in modifiers)
        {
            m.Effect(unit);
        }
    }
    public virtual void OnUnitLeave()
    {
        
    }

    public void ChangeTile(Color newColorSelection)
    {
        if(currentColor == null)
        {
            currentColor = newColorSelection;
            selection.gameObject.SetActive(true);
            selection.transform.localPosition = new Vector3(selection.transform.localPosition.x, 0.505f, selection.transform.localPosition.z);
            selection.color = newColorSelection;
        }
        else
        {
            selection.gameObject.SetActive(true);
            previousColor = currentColor;
            selection.transform.localPosition = new Vector3(selection.transform.localPosition.x, 0.505f, selection.transform.localPosition.z);
            currentColor = newColorSelection;
            selection.color = currentColor;
        }
    }

    public void ChangeTileToDefault()
    {
        if(currentColor != null)
        {
            selection.gameObject.SetActive(false);

            previousColor = currentColor;
            currentObject.transform.localPosition = new Vector3(selection.transform.localPosition.x, 0, selection.transform.localPosition.z);
            selection.color = emptyColor;
            currentColor = emptyColor;
        }
    }

    public Tile CheckSurroundings(Board board)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if(!IsTileValid(board, x, y))
                {
                    return null;
                }
            }
        }

        return this;
    }

    public bool IsTileValid(Board board, int offsetX, int offsetY)
    {
        if (board.GetTile(new Point(pos.x + offsetX, pos.y+offsetY)) != null)
        {
            if(board.GetTile(new Point(pos.x + offsetX, pos.y + offsetY)).content == null)
            {
                return true;
            }
            else
            {
                if(board.GetTile(new Point(pos.x + offsetX, pos.y + offsetY)).content.GetComponent<EnemyUnit>() == null)
                {
                    return false;
                }
                else
                {
                    if(board.GetTile(new Point(pos.x + offsetX, pos.y + offsetY)).content.GetComponent<MinionUnit>() != null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        else
        {
            return false;
        }
    }

    public List<Tile> GetEvent2Area(Board board)
    {
        List<Tile> tiles = new List<Tile>();
        SquareAbilityRange range = GetComponent<SquareAbilityRange>();
        range.SetStartPos(pos);
        tiles = range.GetTilesInRangeWithoutUnit(board, pos);

        foreach(Tile t in tiles)
        {
            t.SetCracks();
        }
        return tiles;
    }
    public bool CheckNextTile(Directions dir, Board board)
    {
        switch (dir)
        {
            case Directions.North:
                if(board.GetTile(new Point(pos.x, pos.y--)) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case Directions.East:
                if (board.GetTile(new Point(pos.x--, pos.y)) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case Directions.South:
                if (board.GetTile(new Point(pos.x, pos.y++)) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case Directions.West:
                if (board.GetTile(new Point(pos.x++, pos.y)) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
    }
}

