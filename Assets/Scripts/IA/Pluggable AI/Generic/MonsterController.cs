using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MonsterController : MonoBehaviour
{
    [Header("Behaviour Tree Variables")]
    public BehaviourTree tree;
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
    public List<PlayerUnit> targetsInRange;
    public List<PlayerUnit> possibleTargets;
    public EnemyActions lastAction = EnemyActions.None;

    [HideInInspector] public List<MonsterAbility> validAbilities;
    [HideInInspector] public MonsterAbility validAttack;
    [Space]
    [Header("Special Abilities")]
    //Bear Abilities
    public GameObject obstacle;
    public List<BearObstacleScript> obstaclesInGame;
    public List<BearObstacleScript> validObstacles;
    public int obstacleLimit;

    //Spider Abilities
    public List<GameObject> minionsPrefab;
    public List<EnemyUnit> minionsInGame;
    public int maxMinions = 3;
    public List<RangeData> minionRangeSpawn;
    public bool hasSpawnedMinionsInLastTurn;
    public bool hasDoneFirstTurn = false;


    [Header("Animation Variables")]
    public bool animPlaying;
    public Animator monsterAnimations;

    private void Start()
    {
        originalTree = tree;
        context = CreateBehaviourTreeContext();
        tree = tree.Clone();
        tree.controller = this;
        tree.Bind(context);
    }
    public void StartMonster()
    {
        context = CreateBehaviourTreeContext();
        tree = originalTree;
        tree = tree.Clone();
        tree.controller = this;
        tree.Bind(context);
        test = true;
        //currentState.UpdateState(this);
    }

    Context CreateBehaviourTreeContext()
    {
        return Context.CreateFromGameObject(gameObject);
    }

    Context context;

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
        EnemyUnit minion = Instantiate(minionsPrefab[indexToSpawn], new Vector3(tileToSpawn.pos.x, 0.5f, tileToSpawn.pos.y), minionsPrefab[indexToSpawn].transform.rotation).GetComponent<EnemyUnit>();
        minionsInGame.Add(minion);
        minion.Place(tileToSpawn);
        minion.Match();
    }

}
