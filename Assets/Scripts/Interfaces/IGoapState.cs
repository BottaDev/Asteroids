using System;
using System.Collections.Generic;

public interface IGoapState
{
    event Action OnNeedsReplan;

    event StateEvent OnEnter;
    event StateEvent OnExit;

    FiniteStateMachine FSM { get; }
    string Name { get; }
    bool HasStarted { get; set; }
    
    Dictionary<string, IGoapState> Transitions { get; set; }

    IGoapState Configure(FiniteStateMachine fsm);

    void Enter(IGoapState from, Dictionary<string, object> transitionParameters);
    void UpdateLoop();
    Dictionary<string, object> Exit(IGoapState to);

    IGoapState ProcessInput();
}

public delegate void StateEvent(IGoapState from, IGoapState to);
