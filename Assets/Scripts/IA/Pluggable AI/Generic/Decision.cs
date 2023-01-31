using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : ScriptableObject
{
    public bool multipleStates;

    public List<Decision> conditionalStates;
    [SerializeField] bool conditionalType;
    public List<MState> trueState;
    [HideInInspector] public List<MState> _trueState;
    public List<MState> falseState;
    [HideInInspector] public List<MState> _falseState;
    
    public abstract bool Decide(MonsterController controller);

    public void CheckConditionalStates(MonsterController controller)
    {
        if(conditionalStates != null)
        {
            foreach(Decision d in conditionalStates)
            {
                if (d.Decide(controller))
                {
                    if (d.conditionalType)
                    {
                        _trueState.Add(d._trueState[0]);
                    }
                    else
                    {
                        _falseState.Add(d.trueState[0]);
                    }
                }
            }
        }
    }

    public void OnEnable()
    {
        ResetDecision();
    }


    public void ResetDecision()
    {
        if(_trueState != null)
        {
            _trueState.Clear();
        }
        foreach (MState state in trueState)
        {
            _trueState.Add(state);
        }


        if(_falseState != null)
        {
            _falseState.Clear();
        }

        foreach (MState state in falseState)
        {
            _falseState.Add(state);
        }
    }
    public void OnDisable()
    {
        
    }

}
