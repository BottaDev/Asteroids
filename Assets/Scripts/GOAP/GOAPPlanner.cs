using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GOAPPlanner 
{
    private const int _WATCHDOG_MAX = 200;

    private int _watchdog;
    
    public IEnumerable<GOAPAction> Run(GOAPState from, GOAPState to, IEnumerable<GOAPAction> actions) 
    {
        _watchdog = _WATCHDOG_MAX;

        var path = new List<GOAPState>();
        
        return CalculateGoap(path);
    }

    public static FiniteStateMachine ConfigureFSM(IEnumerable<GOAPAction> plan, Func<IEnumerator, Coroutine> startCoroutine)
    {
        var prevState = plan.First().linkedState;
            
        var fsm = new FiniteStateMachine(prevState, startCoroutine);

        foreach (var action in plan.Skip(1))
        {
            if (prevState == action.linkedState) continue;
            fsm.AddTransition("On" + action.linkedState.Name, prevState, action.linkedState);
                
            prevState = action.linkedState;
        }

        return fsm;
    }

    private IEnumerable<GOAPAction> CalculateGoap(IEnumerable<GOAPState> sequence) 
    {
        foreach (var act in sequence.Skip(1)) 
        {
            Debug.Log(act);
        }

        Debug.Log("WATCHDOG " + _watchdog);

        return sequence.Skip(1).Select(x => x.generatingAction);
    }
}
