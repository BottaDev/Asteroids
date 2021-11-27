using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoBaseState : MonoBehaviour, IGoapState 
{
    public virtual event Action OnNeedsReplan;
    public virtual event StateEvent OnEnter;
    public virtual event StateEvent OnExit;
    
    public virtual string Name => GetType().Name;

    public virtual bool HasStarted { get; set; }

    public FiniteStateMachine FSM => _fsm;

    public virtual Dictionary<string, IGoapState> Transitions { get; set; } = new Dictionary<string, IGoapState>();

    private FiniteStateMachine _fsm;
    
    public IGoapState Configure(FiniteStateMachine fsm) 
    {
        _fsm =  fsm;
        _fsm.OnActive += OnActive;
        _fsm.OnUnActive += OnUnActive;
        
        return this;
    }

    public virtual void Enter(IGoapState from, Dictionary<string, object> transitionParameters = null) 
    {
        OnEnter?.Invoke(from, this);
        HasStarted = true;
    }

    public virtual Dictionary<string, object> Exit(IGoapState to) 
    {
        OnExit?.Invoke(this, to);
        HasStarted = false;
        return null;
    }

    protected void Replan()
    {
        OnNeedsReplan?.Invoke();
    }
    
    public abstract void UpdateLoop();

    protected virtual void OnActive() {}

    protected virtual void OnUnActive() {}

    public abstract IGoapState ProcessInput();
}