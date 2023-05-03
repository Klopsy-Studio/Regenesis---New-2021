using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;

public class BattleController : StateMachine
{
    public ActionEffectParameters monsterRoar;
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
    public LootUIManager VictoryLootUIManager;
    public LootUIManager DefeatLootUIManager;
    public AbilityTargets targets;
    public GunbladeBullets gunbladeUI;
    public MenuButton droneUI;
    public GameObject questComplete;
    [Space]
    [Header("Weapon trait references")]

    public GameObject bowExtraAttackObject;
    public MenuButton bowExtraAttackMenuButton;
    public GameObject hammerTraitObject;
    public Slider hammerCurrentFury;
    public Slider hammerPreviewFury;
    public Text bowExtraAttackText;
    public GameObject defeatScreen;
    public MenuButton pauseButton;
    public MenuButton resumeButton;

    public MiniStatus miniStatus;
    public TargetIndicator turnArrow;
    [HideInInspector] public TimelineIconUI currentSelectedIcon;
    [SerializeField] Animator sceneTransition;
    public DroneManager droneController;
    [Space]
    [Header("Combat Variables")]
    [HideInInspector] public int attackChosen;
    public List<TimelineElements> timelineElements;
    [Range(0,5)]public int moveCost;
    [Range(0,5)] public int itemCost;

    

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
    [HideInInspector] public bool battleEnded;


    [SerializeField] protected GameObject placeholderCanvas;
    public GameObject placeholderWinScreen;

    public CinemachineVirtualCamera cinemachineCamera;
    public Camera cameraTest;
   

    [Header("Playtesting")]
    [SerializeField] Playtest playtestingFunctions;
    public bool enablePreview;
    public bool enableMiniStatus;
    public bool enableZoom;

    [Header("Unit variables")]
    [SerializeField] 
    [Range(0f, 1f)] float unitFadeValue;
    public bool bowExtraAttack = false;
    public bool endTurnInstantly = false;

    [Header("Timeline Variables")]
    public bool pauseTimeline;
    public bool canToggleTimeline = false;
    [SerializeField] KeyCode toggleTimelineKey;
    [SerializeField] Text pauseText;
    public MenuButton pauseTimelineButton;
    public MenuButton resumeTimelineButton;
    public float huntTime;
    [Header("Zoom Variables")]
    [SerializeField] Animator zoomAnimations;
    [SerializeField] float minCameraZoom = 4f;
    [SerializeField] float maxCameraZoom = 8.5f;


    float currentZoomSize;
    
    int cameraInput
    {
        get
        {
            return _cameraInput;
        }

        set
        {
            _cameraInput = Mathf.Clamp(value, 1, -1);
        }
    }

    int _cameraInput;

    public bool zoomed = false;

    bool zoomIn = false;
    bool zoomOut = false;
    bool zoomVeryOut = false;
    [SerializeField] float zoomSpeed;
    [SerializeField] float preferedZoomSize;
    [SerializeField] AnimationCurve zoomInCurve;

    protected float originalZoomSize;
    float currentTime;

    float fov;
    [SerializeField] [Range(0, 1000)]float sensitivity = 10f;


    [Header("UI button sounds")]
    public int hoverOption;
    public int enterMenu;
    public int exitMenu;
    public int hoverTile;



    public virtual void BeginGame()
    {
        canToggleTimeline = true;
        originalZoomSize = cinemachineCamera.m_Lens.OrthographicSize;
        cinemachineCamera.m_Lens.NearClipPlane = -1f;
        Destroy(placeholderCanvas.gameObject);
        levelData = GameManager.instance.currentMission;
        cameraTest.transparencySortMode = TransparencySortMode.CustomAxis;
        cameraTest.transparencySortAxis = new Vector3(1, 1, 1);
        ChangeState<InitBattleState>();
    }
    //public bool IsInMenu()
    //{
    //    return CurrentState is SelectActionState || CurrentState is SelectAbilityState || CurrentState is SelectItemState || CurrentState is SelectItemState;

    //}

    private void Start()
    {
      
        zoomed = false;
        sceneTransition.SetBool("fadeOut", true);
        //BeginGame();
    }
    public void SetMission(LevelData level)
    {
        GameManager.instance.currentMission = level;
    }

    bool tilesTest = true;
    bool unitUItest = true;
    bool monsterLifeTest = true;
    bool windTest = true;
    public bool playtestToggle = true;

    public bool indicatingTurn;
    public IEnumerator IndicateTurn(Sprite sprite)
    {
        indicatingTurn = true;
        isTimeLineActive = false;
        turnStatusUI.IndicateTurnStatus(sprite);
        yield return new WaitForSeconds(0.8f);
        turnStatusUI.StopTurnStatus();
        yield return new WaitForSeconds(0.8f);
        indicatingTurn = false;
    }

    public void EnableZoom()
    {
        enableZoom = true;
    }

    public void DisableZoom()
    {
        enableZoom = false;
    }

    private void Update()
    {
        huntTime += Time.deltaTime;

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

            if (Input.GetKeyDown(KeyCode.C))
            {
                ChangeState<DefeatState>();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                ChangeState<WinState>();
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
                playerUnits[0].ReceiveDamage(100, true);
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

            if (Input.GetKeyDown(KeyCode.S))
            {
                playerUnits[0].Stun();
            }
        }

        //Pause Timeline With Input
        if (Input.GetKeyDown(toggleTimelineKey) && canToggleTimeline)
        {
            if (pauseTimeline)
            {
                resumeTimelineButton.action.Invoke();
            }
            else
            {
                pauseTimelineButton.action.Invoke();
            }
        }

        if (Input.GetKeyUp(toggleTimelineKey) && canToggleTimeline)
        {
            if (pauseTimeline)
            {
                resumeTimelineButton.onUp.Invoke();
            }
            else
            {
                pauseTimelineButton.onUp.Invoke();

            }
        }


        if (enableZoom)
        {
            fov = cinemachineCamera.m_Lens.OrthographicSize;
            fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity*Time.deltaTime;
            fov = Mathf.Clamp(fov, minCameraZoom, maxCameraZoom);
            cinemachineCamera.m_Lens.OrthographicSize = fov;
        }

    }

    public void ActivateZoom()
    {
        zoomIn = true;
    }
    public void ChangeUIButtons(bool value)
    {
        pauseTimelineButton.canBeSelected = value;
        resumeTimelineButton.canBeSelected = value;
        pauseButton.canBeSelected = value;
        canToggleTimeline = value;
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

    public void OpenDroneTarget()
    {
        ChangeState<SelectDroneTarget>();
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
            isTimeLineActive = false;
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

            isTimeLineActive = false;
            ChangeState<DefeatState>();
        }
    }

    public void MakeAllUnitsDie()
    {
        foreach (PlayerUnit p in playerUnits)
        {
            p.NearDeath();
        }

        CheckAllUnits();
    }

    public void UpdateUnitSprite()
    {
        Debug.Log("Updating");
        if (currentUnit.tile.CheckSpecificDirection(currentTile, Directions.East) || currentUnit.tile.CheckSpecificDirection(currentTile, Directions.South))
        {
            Debug.Log("It's east");
            currentUnit.unitSprite.flipX = true;
        }

        if (currentUnit.tile.CheckSpecificDirection(currentTile, Directions.West) || currentUnit.tile.CheckSpecificDirection(currentTile, Directions.North))
        {
            Debug.Log("It's west");

            currentUnit.unitSprite.flipX = false;
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


    public void ReorganizeIcons()
    {
        foreach(TimelineElements e in timelineElements)
        {
            if(e.iconTimeline != null)
            {
                e.iconTimeline.timelineEnabled = false;
                e.iconTimeline.PutPreviousOnTop();
            }
            
        }
    }

    public void ResetIcons()
    {
        foreach (TimelineElements e in timelineElements)
        {
            if(e.iconTimeline != null)
            {
                if (e.iconTimeline.previousPosition != null)
                {
                    e.iconTimeline.timelineEnabled = true;
                    e.iconTimeline.ResetPrevious();
                }
            }

        }
    }
    public void ToggleTimeline()
    {
        pauseTimeline = !pauseTimeline;
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

    public void ZoomIn()
    {
        if (!zoomOut)
        {
            preferedZoomSize = 4;
            zoomIn = true;
        }
    }

    public void ZoomOut()
    {
        if (!zoomIn)
        {
            zoomOut = true;
        }
    }

    public void ZoomVeryOut()
    {
        zoomVeryOut = true;
        preferedZoomSize = 9;
    
    }
    public void SetBowExtraAttack()
    {
        bowExtraAttack = !bowExtraAttack;

        if (bowExtraAttack)
        {
            currentUnit.EnableBowTrait();
        }

        else
        {
            currentUnit.ResetWeaponTraits();

        }
    }

    public void ResetBowExtraAttack()
    {
        bowExtraAttack = false;
        bowExtraAttackMenuButton.SetDefaultSprite();
        currentUnit.ResetWeaponTraits();
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

    public void StartAction()
    {
        pauseTimelineButton.canBeSelected = false;
        resumeTimelineButton.canBeSelected = false;
        pauseTimelineButton.onUp.Invoke();
    }

    public void FinishAction()
    {
        pauseTimelineButton.canBeSelected = true;
        resumeTimelineButton.canBeSelected = true;
        resumeTimelineButton.onUp.Invoke();
    }


    public void CheckAbilities()
    {
        for (int i = 0; i < currentUnit.weapon.Abilities.Length; i++)
        {
            if (currentUnit.weapon.Abilities[i] != null)
            {
                if (currentUnit.weapon.EquipmentType == KitType.Gunblade)
                {
                    if (currentUnit.weapon.Abilities[i].CanDoAbility(currentUnit.actionsPerTurn, currentUnit))
                    {
                        abilitySelectionUI.EnableSelectAbilty(i);
                    }
                    else
                    {
                        abilitySelectionUI.DisableSelectAbilty(i);
                    }
                }
                else
                {
                    if (currentUnit.weapon.Abilities[i].CanDoAbility(currentUnit.actionsPerTurn))
                    {
                        abilitySelectionUI.EnableSelectAbilty(i);

                    }
                    else
                    {
                        abilitySelectionUI.DisableSelectAbilty(i);
                    }
                }
            }
        }
    }
}
