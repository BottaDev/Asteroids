using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FiniteStateMachine 
{
    private const int _MAX_TRANSITIONS_PER_FRAME = 3;

    public delegate void StateEvent(IGoapState from, IGoapState to);

    public event Action OnActive;
    public event Action OnUnActive;

    public IGoapState CurrentState { get; private set; }
    private List<IGoapState> _allStates;

    private Func<IEnumerator, Coroutine> _startCoroutine;

    private bool _isActive;

    public FiniteStateMachine(IGoapState initialState, Func<IEnumerator, Coroutine> startCoroutine)
    {
        CurrentState = initialState.Configure(this);
        _allStates = new List<IGoapState>{CurrentState};

        _startCoroutine = startCoroutine;
    }

    public IEnumerator Update()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        while (Active) 
        {
            if (stopwatch.ElapsedMilliseconds >= 1f / 60f * 1000f)
            {
                yield return null;
                stopwatch.Restart();
            }
            
            CurrentState.UpdateLoop();
            
            var nextState = CurrentState.ProcessInput();
            var stateTransitions = 0;
            
            while (nextState != CurrentState && stateTransitions < _MAX_TRANSITIONS_PER_FRAME) 
            {
                if (stopwatch.ElapsedMilliseconds >= 1f / 60f * 1000f)
                {
                    yield return null;
                    stopwatch.Restart();
                }
                
                var previousState = CurrentState;
                var transitionParameters = CurrentState.Exit(nextState);

                //Debug.Log("Exiting state '" + CurrentState.Name + "' to state '" + nextState.Name + "'.");
                
                CurrentState = nextState;
                CurrentState.Enter(previousState, transitionParameters);

                nextState = CurrentState.ProcessInput();

                stateTransitions++;
            }
            
            yield return null;
        }
    }

    public FiniteStateMachine AddTransition(string transitionName, IGoapState from, IGoapState to) 
    {
        from.Configure(this);
        to.Configure(this);
        
        if (from.Transitions == null) 
            from.Transitions = new Dictionary<string, IGoapState>();

        if (!from.Transitions.ContainsKey(transitionName)) 
        {
            from.Transitions.Add(transitionName, to);
            if (!_allStates.Contains(from)) _allStates.Add(from);
            if (!_allStates.Contains(to))   _allStates.Add(to);
        }

        return this;
    }

    public FiniteStateMachine Clear() 
    {
        foreach (var state in _allStates) 
        {
            state.Transitions = new Dictionary<string, IGoapState>();
            state.HasStarted  = false;
        }

        return this;
    }
    
    public bool Active 
    {
        get { return _isActive; }
        
        set 
        {
            if (_isActive == value) 
                return;
            _isActive = value;
            
            if (_isActive) 
            {
                if (!CurrentState.HasStarted) CurrentState.Enter(CurrentState, null);
                _startCoroutine(Update());
                OnActive?.Invoke();
            }
            else
                OnUnActive?.Invoke();
        }
    }
}
