using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PluggableAI/m_state")]
public class MState : ScriptableObject
{
    
    public Color sceneGizmoColor = Color.grey;
    public Transition transition;
    public Action actions;
    //public Action[] actions;

    [SerializeField] bool isDecisionRandom;
    

    public void UpdateState(MonsterController controller)
    {
        Debug.Log("Current State: "+this.name);

        if(actions != null)
        {
            actions.Act(controller);
        }
        else
        {
            OnExit(controller);
        }
    }

    //private void DoActions(TestMonsterController controller)
    //{
    //    for (int i = 0; i < actions.Length; i++)
    //    {
    //        actions[i].Act(controller);
    //    }
    //}

    public void CheckTransitions(MonsterController controller)
    {
        foreach(Decision d in transition.decision)
        {
            d.CheckConditionalStates(controller);
        }

        if (!isDecisionRandom)
        {
            if(transition.decision.Length > 1)
            {
                for (int i = 0; i < transition.decision.Length; i++)
                {
                    bool decisionSucceeded = transition.decision[i].Decide(controller);

                    if (decisionSucceeded)
                    {
                        if (!transition.decision[i].multipleStates)
                        {
                            controller.TransitionToState(transition.decision[i]._trueState[0]);
                        }

                        else
                        {
                            int random = Random.Range(0, transition.decision[i]._trueState.Count);
                            controller.TransitionToState(transition.decision[i]._trueState[random]);
                        }
                    }
                }
            }

            else
            {
                if (transition.decision[0].Decide(controller))
                {
                    if (!transition.decision[0].multipleStates)
                    {
                        controller.TransitionToState(transition.decision[0]._trueState[0]);
                    }
                    else
                    {
                        int random = Random.Range(0, transition.decision[0]._trueState.Count);
                        controller.TransitionToState(transition.decision[0]._trueState[random]);
                    }
                }
                else
                {
                    if (!transition.decision[0].multipleStates)
                    {
                        controller.TransitionToState(transition.decision[0]._falseState[0]);
                    }
                    else
                    {
                        int random = Random.Range(0, transition.decision[0]._falseState.Count);
                        controller.TransitionToState(transition.decision[0]._falseState[random]);
                    }
                }
            }
        }

        else
        {
            if(transition.decision.Length > 1)
            {
                int random = Random.Range(0, 2);

                if(random == 0)
                {
                    controller.TransitionToState(transition.decision[0]._trueState[0]);
                }
                else if(random == 1)
                {
                    controller.TransitionToState(transition.decision[1]._trueState[0]);
                }
            }
        }

        for (int i = 0; i < transition.decision.Length; i++)
        {
            transition.decision[i].ResetDecision();
        }

    }


    protected virtual void OnExit(MonsterController controller)
    {
        controller.currentState.CheckTransitions(controller);
        controller.currentState.UpdateState(controller);
    }
}
