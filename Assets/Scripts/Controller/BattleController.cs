using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;

public class BattleController : StateMachine
{
    [HideInInspector] public MonsterController monsterController;

    [HideInInspector] public bool isTimeLineActive = true;
    public CameraRig cameraRig;
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public TileSelectionToggle tileSelectionToggle;
    public GameObject tileSpriteGhostImage;
    public Point pos;

    public ConsumableBackpack backpackInventory;
    //public ConsumableInventoryDemo inventory;

    public LootSystem lootSystem;
    public PlayerUnit currentUnit;
    public EnemyUnit currentEnemyUnit;
    [HideInInspector] public EnemyController currentEnemyController;
   


    [HideInInspector] public ItemElements currentItem;
    [HideInInspector] public int itemIndexToRemove;

    [Space]
    [Header("Units lists")]
    public List<Unit> unitsInGame;
    public List<Unit> unitsWithActions;

    public List<Unit> playerUnits;
    public List<Unit> enemyUnits;

    [Space]
    [Header("UI references")]
    public OptionSelection actionSelectionUI;
    public OptionSelection abilitySelectionUI;
    public OptionSelection itemSelectionUI;
    public UnitStatusList unitStatusUI;
    public TurnStatusUI turnStatusUI;
    public AbilityDetailsUI abilityDetailsUI;
    public AttackUI attackUI;
    public SpriteRenderer ghostImage;
    public TimelineUI timelineUI;
    public ExpandedUnitStatus expandedUnitStatus;
    public UIController uiController;
    public LootUIManager lootUIManager;
    public AbilityTargets targets;
    public GameObject bowExtraAttackObject;
    public Text bowExtraAttackText;

    [SerializeField] Animator sceneTransition;
    [Space]
    [Header("Combat Variables")]
    [HideInInspector] public int attackChosen;
    public List<TimelineElements> timelineElements;
    public int moveCost;
    public int itemCost;
    public Tile currentTile { get { return board.GetTile(pos); } }
    [Space]
    [Header("Events")]
    public RealTimeEvents environmentEvent;
    [HideInInspector] public MonsterEvent currentMonsterEvent;
    [HideInInspector] public HunterEvent currentHunterEvent;

    //Item variables
    [HideInInspector] public int itemChosen;

  

    [HideInInspector] public bool moveActionSelector = false;
    [HideInInspector] public bool moveAbilitySelector = false;
    [HideInInspector] public bool moveItemSelector = false;
    [HideInInspector] public bool win;
    [HideInInspector] public bool lose;


    [SerializeField] GameObject placeholderCanvas;
    public GameObject placeholderWinScreen;

    public CinemachineVirtualCamera cinemachineCamera;

    [Header("Playtesting")]
    [SerializeField] Playtest playtestingFunctions;

    [Header("Unit variables")]
    [SerializeField] 
    [Range(0f, 1f)] float unitFadeValue;
    public bool bowExtraAttack = false;
    public bool endTurnInstantly = false;

    [Header("Timeline Variables")]
    public bool pauseTimeline;
    [SerializeField] Text pauseText;
    [SerializeField] Text pauseTextButton;
    public void BeginGame()
    {
        cinemachineCamera.m_Lens.NearClipPlane = -1f;
        Destroy(placeholderCanvas.gameObject);
        levelData = GameManager.instance.currentMission;
        
        ChangeState<InitBattleState>();
    }
    //public bool IsInMenu()
    //{
    //    return CurrentState is SelectActionState || CurrentState is SelectAbilityState || CurrentState is SelectItemState || CurrentState is SelectItemState;

    //}

    private void Start()
    {
        sceneTransition.SetBool("fadeOut", true);
    }

    bool tilesTest = true;
    bool unitUItest = true;
    bool monsterLifeTest = true;
    bool windTest = true;
    public bool playtestToggle = true;
    private void Update()
    {
        if (playtestToggle)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (uiController.gameObject.activeSelf)
                {
                    uiController.gameObject.SetActive(false);
                }
                else
                {
                    uiController.gameObject.SetActive(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (tilesTest)
                {
                    board.ActivateTileSelectionToggle();
                    tilesTest = false;
                }
                else
                {
                    board.DeactivateTileSelection();
                    tilesTest = true;
                }

            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                if (currentUnit != null)
                {
                    if (unitUItest)
                    {
                        currentUnit.playerUI.actionPointsObject.SetActive(true);
                        unitUItest = false;
                    }
                    else
                    {
                        currentUnit.playerUI.actionPointsObject.SetActive(false);
                        unitUItest = true;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                foreach (PlayerUnit u in playerUnits)
                {
                    isTimeLineActive = false;
                    u.animations.SetNearDeath();
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                foreach (PlayerUnit u in playerUnits)
                {
                    isTimeLineActive = false;
                    u.animations.SetDeath();
                }
            }
            playtestingFunctions.elements = timelineElements;

            if (Input.GetKeyDown(KeyCode.U))
            {
                enemyUnits[0].health = 1;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                if (windTest)
                {
                    environmentEvent.fTimelineVelocity = 20;
                    windTest = false;
                }
                else
                {
                    environmentEvent.fTimelineVelocity = 0;

                    windTest = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                board.toggleTileActivation = !board.toggleTileActivation;
            }
        }
        //Disable UI 
       

    }

    public virtual void SelectTile(Point p)
    {
        if (pos == p || !board.playableTiles.ContainsKey(p))
        {
            return;
        }

        pos = p;
        tileSelectionIndicator.localPosition = board.playableTiles[p].center;
    }


    public void ActivateTileSelector()
    {
        tileSelectionIndicator.gameObject.SetActive(true);
    }

    public void DeactivateTileSelector()
    {
        tileSelectionIndicator.gameObject.SetActive(false);
    }

    public void CheckAllUnits()
    {
        if(playerUnits.Count == 0)
        {
            ChangeState<DefeatState>();
        }

        else
        {
            foreach(PlayerUnit p in playerUnits)
            {
                if (!p.isNearDeath)
                {
                    return;
                }
            }

            ChangeState<DefeatState>();
        }
    }

    public void FadeUnits()
    {
        foreach (Unit u in unitsInGame)
        {
            if (u == currentUnit)
                continue;

            u.unitSprite.color = new Color(u.unitSprite.color.r, u.unitSprite.color.g, u.unitSprite.color.b, unitFadeValue);
        }
    }

    public void PauseOrResumeTimeline()
    {
        pauseTimeline = !pauseTimeline;

        if (!pauseTimeline)
        {
            pauseText.color = Color.green;
            pauseText.text = "Active";
            pauseTextButton.text = "Pause Timeline";
        }
        else
        {
            pauseText.color = Color.red;
            pauseText.text = "Paused";

            pauseTextButton.text = "Resume Timeline";


        }
    }
    public void ResetUnits()
    {
        foreach (Unit u in unitsInGame)
        {
            if (u == currentUnit)
                continue;

            u.unitSprite.color = new Color(u.unitSprite.color.r, u.unitSprite.color.g, u.unitSprite.color.b, 1f);
        }
    }


    public void SetBowExtraAttack()
    {
        bowExtraAttack = !bowExtraAttack;

        if (bowExtraAttack)
        {
            bowExtraAttackText.text = "Remove Extra Attack";
        }

        else
        {
            bowExtraAttackText.text = "Set Extra Attack";
        }
    }

    public void ResetBowExtraAttack()
    {
        bowExtraAttack = false;
        bowExtraAttackText.text = "Set Extra Attack";

    }

    public void PlayCorotuine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void ReturnToCamp()
    {
        sceneTransition.SetTrigger("fadeIn");
        GameManager.instance.sceneToLoad = "CampScene";
        Invoke("LoadingScreen", 1.5f);
    }


    public void LoadingScreen()
    {
        SceneManager.LoadScene("LoadingScreen");
    }
}
