using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public bool isUpdatingState = false;
   
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
    public GameObject obstacle;
    public List<BearObstacleScript> obstaclesInGame;
    public List<BearObstacleScript> validObstacles;
    public int obstacleLimit;
    // Update is called once per frame
    //void Update()
    //{
    //    currentState.UpdateState(this);
    //}

    [Header("Animation Variables")]
    public bool animPlaying;
    public Animator monsterAnimations;
    public void StartMonster()
    {
        currentState.UpdateState(this);
    }



    protected void OnDrawGizmos()
    {
        if (currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, 2);
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
}
