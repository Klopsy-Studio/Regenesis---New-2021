using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public virtual State CurrentState
    {
        get { return _currentState; }
        set { Transition(value); } //Whenever we change _currentState, it will first execute Transition method
    }

    protected State _currentState;
    protected bool _inTransition;

    public virtual T GetState<T>() where T : State //Add 
    {
        T target = GetComponent<T>();
        if (target == null)
            target = gameObject.AddComponent<T>();
        return target;
    }

    public virtual void ChangeState<T>() where T : State
    {
        CurrentState = GetState<T>();
    }

    protected virtual void Transition(State value)
    {
        if (_currentState == value || _inTransition) //if you try to set the current state to the state ii already is, then it just exists.
            return;                                 //and you can not set a new state during a transition

        _inTransition = true;

        if (_currentState != null) 
            _currentState.Exit();

        _currentState = value;

        if (_currentState != null)
            _currentState.Enter();

        _inTransition = false;
    }
}
