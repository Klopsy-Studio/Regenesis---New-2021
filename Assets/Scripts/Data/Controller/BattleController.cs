using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Linq;
using UnityEngine.UI;

public class BattleController : StateMachine
{
    [HideInInspector] public CurrentEntityTurn currentEntity;
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
    public ContextControls battleContextControls;

    [Header("Quest Banners")]
    public GameObject questComplete;
    public GameObject questFailed;
    public GameObject questEscaped;

    public GameObject partyIconParent;
    public GameObject partyIconPrefab;
    [Space]
    [Header("Weapon trait references")]

    public GameObject bowExtraAttackObject;
    public MenuButton bowExtraAttackMenuButton;
    public GameObject hammerTraitObject;
    public Slider hammerCurrentFury;
    public Slider hammerPreviewFury;
    public Text bowExtraAttackText;
    public GameObject defeatScreen;

    public Pause pause;
    public Animator pauseAnimations;
    public bool canPause;

    public MiniStatus miniStatus;
    public TargetIndicator turnArrow;
    [HideInInspector] public TimelineIconUI currentSelectedIcon;
    [SerializeField] protected Animator sceneTransition;
    public DroneManager droneController;
    [Space]
    [Header("Combat Variables")]
    [HideInInspector] public int attackChosen;
    [HideInInspector] public LevelManager currentLevelManager;
    public List<TimelineElements> timelineElements;
    public List<TimelineElements> orderedTimelineSlements;

    [Header("Combat costs")]
    [Range(0, 5)] public int moveCost;
    [Range(0, 5)] public int itemCost;

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
    public Camera uiCamera;
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
    [SerializeField] Color disabledTimelineColor;
    [SerializeField] Color defaultTimelineColor;

    [Header("Zoom Variables")]
    [SerializeField] Animator zoomAnimations;
    [SerializeField] float minCameraZoom = 4f;
    [SerializeField] float maxCameraZoom = 8.5f;


    float currentZoomSize;

    [SerializeField] float deadzoneWidth = 0;
    [SerializeField] float deadzoneHeight = 0;

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
    [SerializeField] [Range(0, 1000)] float sensitivity = 10f;


    [Header("UI button sounds")]
    public int hoverOption;
    public int enterMenu;
    public int exitMenu;
    public int hoverTile;


    public void ActivateUnitTooltip()
    {
        foreach(Unit u in unitsInGame)
        {
            u.iconTimeline.GetComponent<ToolTipTrigger>().allowTooltip = true;
        }
    }

    public void DeactivateUnitTooltip()
    {
        foreach (Unit u in unitsInGame)
        {
            u.iconTimeline.GetComponent<ToolTipTrigger>().allowTooltip = false;
        }
    }
    public virtual void BeginGame()
    {
        AudioManager.instance.ResetSound("Music");
        originalZoomSize = cinemachineCamera.m_Lens.OrthographicSize;
        cinemachineCamera.m_Lens.NearClipPlane = -1f;
        levelData = GameManager.instance.currentMission;
        cameraTest.transparencySortMode = TransparencySortMode.CustomAxis;
        cameraTest.transparencySortAxis = new Vector3(1, 1, 1);
        ChangeState<InitBattleState>();
    }
    //public bool IsInMenu()
    //{
    //    return CurrentState is SelectActionState || CurrentState is SelectAbilityState || CurrentState is SelectItemState || CurrentState is SelectItemState;

    //}

    public void DisableResumeTimelineButton()
    {
        resumeTimelineButton.GetComponent<Image>().color = disabledTimelineColor;

    }

    public void EnableResumeTimelineButton()
    {
        resumeTimelineButton.GetComponent<Image>().color = defaultTimelineColor;
    }
    public virtual void Start()
    {
        zoomed = false;
        sceneTransition.SetBool("fadeOut", true);
        currentEntity = CurrentEntityTurn.None;
        isTimeLineActive = false;
        BeginGame();
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

    public void RenameInspectInUnits(string newText)
    {
        foreach (Unit u in unitsInGame)
        {
            u.description.content = newText;
        }
    }
    private void Update()
    {
        //Calculating hunt time
        huntTime += Time.deltaTime;

        #region Playtest functions

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

            if (Input.GetKeyDown(KeyCode.E))
            {
                playerUnits[1].ReceiveDamage(100, true);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                foreach (PlayerUnit u in playerUnits)
                {
                    isTimeLineActive = false;
                    u.animations.SetDeath();
                }
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                foreach (PlayerUnit u in playerUnits)
                {

                    u.animations.SetNearDeath();
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
            if (Input.GetKeyDown(KeyCode.B))
            {
                enemyUnits[0].GetComponent<EnemyUnit>().UpdateEnemyUnitSprite();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                playerUnits[0].Stun();
            }
        }

        #endregion

        #region Pause and resume timeline
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
                ChangeCurrentControls("Pause");
            }
            else
            {
                pauseTimelineButton.onUp.Invoke();
                ChangeCurrentControls("Resume");
            }
        }

        #endregion

        #region Zoom in and out

        if (enableZoom)
        {

            fov = cinemachineCamera.m_Lens.OrthographicSize;
            fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity * Time.deltaTime;
            fov = Mathf.Clamp(fov, minCameraZoom, maxCameraZoom);
            cinemachineCamera.m_Lens.OrthographicSize = fov;

        }

        #endregion

        //Create an ordered timeline elements list
        if(!pauseTimeline)
            SortTimelineList();

    }

    public void ScrollZoom(float cameraFov)
    {
        float fov = cameraFov;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity * Time.deltaTime;
        fov = Mathf.Clamp(fov, minCameraZoom, maxCameraZoom);
        cameraFov = fov;
    }
    public void ActivateZoom()
    {
        zoomIn = true;
    }
    public void ChangeUIButtons(bool value)
    {
        pauseTimelineButton.canBeSelected = value;
        resumeTimelineButton.canBeSelected = value;
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

    public void CurrentUnitPreviewDroneCost()
    {
        if (currentUnit.actionsPerTurn >= 1)
        {
            currentUnit.playerUI.PreviewActionCost(1);
        }
    }

    public void CurrentUnitShowActionPoints()
    {
        currentUnit.playerUI.ShowActionPoints();
    }
    public void OpenDroneTarget()
    {
        if (currentUnit.actionsPerTurn >= 1)
        {
            ChangeState<SelectDroneTarget>();
        }

        else
        {
            AudioManager.instance.Play("Boton12");
        }
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
        if (playerUnits.Count == 0)
        {
            isTimeLineActive = false;

            AudioManager.instance.FadeOut("MainTheme");
            ChangeState<DefeatState>();
        }

        else
        {
            foreach (PlayerUnit p in playerUnits)
            {
                if (!p.isNearDeath)
                {
                    return;
                }
            }

            isTimeLineActive = false;
            AudioManager.instance.FadeOut("MainTheme");
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

        else if (currentUnit.tile.CheckSpecificDirection(currentTile, Directions.West) || currentUnit.tile.CheckSpecificDirection(currentTile, Directions.North))
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

            u.SetUnitFade(true);
            //u.unitSprite.color = new Color(u.unitSprite.color.r, u.unitSprite.color.g, u.unitSprite.color.b, unitFadeValue);
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

            u.SetUnitFade(false);
            //u.unitSprite.color = new Color(u.unitSprite.color.r, u.unitSprite.color.g, u.unitSprite.color.b, 1f);
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

    public void WinLevel()
    {
        levelData.CompleteHunt();
    }
    public void PlayCorotuine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void ReturnToCamp()
    {
        if(battleContextControls != null)
        {
            battleContextControls.gameObject.SetActive(false);
        }

        sceneTransition.SetBool("fadeInPause", true);
        uiController.gameObject.SetActive(false);
        GameManager.instance.sceneToLoad = "CampScene";
        AudioManager.instance.FadeOut("Music");
        AudioManager.instance.FadeOut("MainTheme");
        AudioManager.instance.FadeOut("LoseTheme");

        StartCoroutine(LoadingScreen());
    }

    public void ReturnToCampDelay()
    {
        if (battleContextControls != null)
        {
            battleContextControls.gameObject.SetActive(false);
        }

        sceneTransition.SetBool("fadeInPause", true);
        uiController.gameObject.SetActive(false);
        GameManager.instance.sceneToLoad = "CampScene";
        AudioManager.instance.FadeOut("Music");
        AudioManager.instance.FadeOut("MainTheme");
        AudioManager.instance.FadeOut("LoseTheme");

        Invoke("LoadingCall", 3f);
    }

    public void LoadingCall()
    {
        StartCoroutine(LoadingScreen());

    }
    public void BeginEscape()
    {
        StartCoroutine(ReturnToCampRoutine());
    }
    public IEnumerator ReturnToCampRoutine()
    {
        actionSelectionUI.gameObject.SetActive(false);
        itemSelectionUI.gameObject.SetActive(false);
        abilitySelectionUI.gameObject.SetActive(false);
        targets.gameObject.SetActive(false);
        partyIconParent.gameObject.SetActive(false);
        turnStatusUI.gear.gameObject.SetActive(false);
        timelineUI.gameObject.SetActive(false);
        miniStatus.gameObject.SetActive(false);
        partyIconParent.SetActive(false);
        ActionEffect.instance.BlackAndWhite();

        yield return new WaitForSecondsRealtime(1f);

        questEscaped.gameObject.SetActive(true);


        yield return new WaitForSecondsRealtime(2f);
        sceneTransition.SetBool("fadeInPause", true);

        yield return new WaitForSecondsRealtime(2f);

        ReturnToCamp();
    }
    IEnumerator LoadingScreen()
    {
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1;
        SceneManager.LoadScene("LoadingScreen");
    }

    public void StartAction()
    {
        pauseTimelineButton.canBeSelected = false;
        resumeTimelineButton.canBeSelected = false;
    }

    public void FinishAction()
    {
        pauseTimelineButton.canBeSelected = true;
        resumeTimelineButton.canBeSelected = true;
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

    public void SetCameraStill()
    {
        CinemachineFramingTransposer transposer = cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        if(transposer == null)
        {
            Debug.Log("Composer is null");
            return;
        }
        transposer.m_DeadZoneHeight = deadzoneHeight;
        transposer.m_DeadZoneWidth = deadzoneWidth;
    }

    public void ResetCamera()
    {
        CinemachineFramingTransposer transposer = cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (transposer == null)
        {
            Debug.Log("Composer is null");
            return;
        }

        transposer.m_DeadZoneHeight = 0;
        transposer.m_DeadZoneWidth = 0;
    }

    public void ChangeCurrentControls(string controlsID)
    {
        battleContextControls.ChangeCurrentWindow(controlsID);
        Debug.Log("Changed to " + controlsID);
    }

    public void DeactivateCurrentControls()
    {
        battleContextControls.DeactivateCurrentWindow();
    }


    void SortTimelineList()
    {
        orderedTimelineSlements = timelineElements.OrderByDescending(x => x.timelineFill).ToList();
    }
}
