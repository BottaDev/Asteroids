using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy : MonoBehaviour
{
    [Header("GOAP States")]
    public ChaseState chaseState;

    private FiniteStateMachine _fsm;
    private float _lastReplanTime;
    private float _replanRate = .5f;
    
    private void Start() 
    {
        chaseState.OnNeedsReplan += OnReplan;
        
        PlanAndExecute();
    }

    private void PlanAndExecute() 
    {
        var actions = new List<GOAPAction>
        {
            new GOAPAction("Chase")
                .Pre("isPlayerInSight", true)
                .Effect("isPlayerNear",    true)
                .LinkedState(chaseState),
        };
        
        var from = new GOAPState();
        from.values["isPlayerAlive"] = true;

        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;

        var planner = new GOAPPlanner();

        var plan = planner.Run(from, to, actions);

        ConfigureFsm(plan);
    }

    private void OnReplan() 
    {
        if (Time.time >= _lastReplanTime + _replanRate)
            _lastReplanTime = Time.time;
        else
            return;

        var actions = new List<GOAPAction>
        {
            new GOAPAction("Chase")
                .Pre("isPlayerInSight", true)
                .Effect("isPlayerNear", true)
                .LinkedState(chaseState),
        };

        // TODO: Revisar...
        var from = new GOAPState();
        from.values["isPlayerInSight"] = false;
        from.values["isPlayerNear"] = false;
        from.values["isPlayerAlive"] = true;

        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;

        var planner = new GOAPPlanner();

        var plan = planner.Run(from, to, actions);
        
        ConfigureFsm(plan);
    }

    private void ConfigureFsm(IEnumerable<GOAPAction> plan) 
    {
        _fsm = GOAPPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

}
