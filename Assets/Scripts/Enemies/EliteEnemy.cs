using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy : MonoBehaviour
{
    public ChaseState chaseState;
    public AttackState attackState;

    public Pool<EnemyBullet> bulletPool;
    
    [HideInInspector] public PlayerModel player;
    
    private FiniteStateMachine _fsm;

    private float _lastReplanTime;
    private float _replanRate = .5f;
    
    private void Start() 
    {
        chaseState.OnNeedsReplan += OnReplan;
        attackState.OnNeedsReplan += OnReplan;
        
        player = FindObjectOfType<PlayerModel>();
        
        EnemyBulletBuilder bulletBuilder = new EnemyBulletBuilder();
        bulletBuilder.Configure(attackState.bulletSpeed, attackState.timeToDestroy);
        bulletPool = new Pool<EnemyBullet>(bulletBuilder.Build, EnemyBullet.TurnOn, EnemyBullet.TurnOff, 5);
        
        PlanAndExecute();
    }

    private void PlanAndExecute() 
    {
        var actions = new List<GOAPAction>
        {
            new GOAPAction("Chase")
                .Effect("isPlayerNear",true)
                .LinkedState(chaseState),
            
            new GOAPAction("Attack")
                .Pre("isPlayerNear",true)
                .Effect("isPlayerAlive",false)
                .LinkedState(attackState),
        };
        
        var from = new GOAPState();
        from.values["isPlayerNear"] = false;
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

        PlanAndExecute();
    }

    private void ConfigureFsm(IEnumerable<GOAPAction> plan) 
    {
        //Debug.Log("Completed Plan");
        _fsm = GOAPPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }
}
