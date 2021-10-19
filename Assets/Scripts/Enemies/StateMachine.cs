using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private Dictionary<string, IState> _stateDictionary = new Dictionary<string, IState>();

    IState currentState;

    public void OnUpdate()
    {
        currentState?.OnUpdate();
    }


    public void ChangeState(string id)
    {
        currentState?.OnExit();
        currentState = _stateDictionary[id];
        currentState?.OnEnter();
    }

    public void AddState(string id, IState state)
    {
        _stateDictionary.Add(id, state);
    }

    public void RemoveState(string id)
    {
        _stateDictionary.Remove(id);
    }

    public void ClearStates()
    {
        _stateDictionary.Clear();
    }
}

public interface IState
{
    void OnEnter();
    void OnExit();
    void OnUpdate();
}
