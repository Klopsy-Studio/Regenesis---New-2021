using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MonsterController : MonoBehaviour
{
    [Header("Behaviour Tree Variables")]
    public BehaviourTree tree;
    [SerializeField] BehaviourTreeRunner testRunner;
    public bool turnFinished;
    //0 stands for no last sequence
    public int lastSequence = 0;
    public int currentSequence;
    BehaviourTree originalTree;
    public bool test = false;
    public bool behaviourTest = false;
    public bool isUpdatingState = false;
    [HideInInspector] public Tile tileToMove;
    public MState currentState;

    public MState startState;

    public BattleController battleController;
    public EnemyUnit currentEnemy;
    public PlayerUnit target;
    public PlayerUnit previousTarget;
    public List<PlayerUnit> targetsInRange;
    public List<PlayerUnit> possibleTargets;
    public EnemyActions lastAction = EnemyActions.None;

    [HideInInspector] public List<MonsterAbility> validAbilities;
    [HideInInspector] public MonsterAbility validAttack;
    [Space]
    [Header("Special Bear Variables")]
    [Space]
    //Bear Abilities
    public GameObject obstacle;
    public List<BearObstacleScript> obstaclesInGame;
    public List<BearObstacleScript> validObstacles;
    public BearObstacleScript chosenObstacle;
    public int obstacleLimit;
    [Space]
    [Space]

    [Header("Special Spider Variables")]
    [Space]
    public List<RangeData> spiderMonsterMovementRange;
    public List<GameObject> minionsPrefab;
    public List<EnemyUnit> minionsInGame;
    public int maxMinions = 3;
    public List<RangeData> minionRangeSpawn;
    public bool hasSpawnedMinionsInLastTurn;
    public bool hasDoneFirstTurn = false;
    public bool hasEvolved;
    public int turnsAlive;
    [Space]

    [Header("Animation Variables")]
    public bool animPlaying;
    public Animator monsterAnimations;

    private void Start()
    {
        originalTree = tree;
        context = CreateBehaviourTreeContext();
        tree = originalTree;
        tree = tree.Clone();
        tree.controller = this;
        tree.Bind(context);
        testRunner.tree = tree;
        //context = CreateBehaviourTreeContext();
        //tree = tree.Clone();
        //tree.controller = this;
        //tree.Bind(context);
    }
    public void StartMonster()
    {
        test = true;
        tree.rootNode.state = Node.State.Running;
        tree.ResetAllNodes();
        turnFinished = false;
        //currentState.UpdateState(this);
    }

    Context CreateBehaviourTreeContext()
    {
        return Context.CreateFromGameObject(gameObject);
    }

    [SerializeField] Context context;

    protected void OnDrawGizmos()
    {
        if (currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, 2);
        }
    }

    public void Update()
    {
        if (test)
        {
            tree.Update();
        }
    }
    public void TransitionToState(MState nextState)
    {
        if (nextState != currentState)
        {
            currentState = nextState;
        }
    }


    public void AnimationBegin()
    {
        animPlaying = true;
    }

    public void AnimationEnd()
    {

    }
    public virtual T GetRange<T>() where T : AbilityRange
    {
        T target = GetComponent<T>();
        if (target == null)
        {
            target = gameObject.AddComponent<T>();
            
        }
        target.unit = currentEnemy;
        return target;
    }

    public void CallCoroutine (IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public virtual MonsterAbility ChooseRandomAttack()
    {
        return null;
    }

    public virtual MonsterAbility ChooseSpecificAttack()
    {
        return null;
    }

    public BearObstacleScript SpawnObstacle(Tile tileToPlaceObstacle)
    {
        return Instantiate(obstacle, new Vector3(tileToPlaceObstacle.pos.x, 1, tileToPlaceObstacle.pos.y), obstacle.transform.rotation).GetComponent<BearObstacleScript>();
    }

    public MonsterEvent SpawnEvent(MonsterEvent e)
    {
        e.controller = this;
        return Instantiate(e);
    }

    public void SpawnMinion(int indexToSpawn, Tile tileToSpawn)
    {
        EnemyUnit minion = Instantiate(minionsPrefab[indexToSpawn], new Vector3(tileToSpawn.pos.x, minionsPrefab[indexToSpawn].transform.position.y, tileToSpawn.pos.y), minionsPrefab[indexToSpawn].transform.rotation).GetComponent<EnemyUnit>();
        minion.controller = battleController;
        minionsInGame.Add(minion);
        battleController.enemyUnits.Add(minion);
        battleController.timelineElements.Add(minion);
        battleController.unitsInGame.Add(minion); 
        minion.GetComponent<MinionUnit>().parent = this;
        minion.timelineFill = Random.Range(0, 15);
        minion.Place(tileToSpawn);
        minion.Match();
    }


    public PlayerUnit GetClosestHunter()
    {
        float maxDistance = 0;
        PlayerUnit closestHunter = new PlayerUnit();
        foreach (PlayerUnit unit in battleController.playerUnits)
        {
            if(closestHunter == null)
            {
                maxDistance = Vector3.Distance(unit.transform.position, transform.position);
                closestHunter = unit;
            }
            else
            {
                if (Vector3.Distance(unit.transform.position, transform.position) <= maxDistance || maxDistance == 0)
                {
                    maxDistance = Vector3.Distance(closestHunter.transform.position, transform.position);
                    closestHunter = unit;
                }
            }
            
        }


        return closestHunter;
    }

}
