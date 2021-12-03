using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy : MonoBehaviour
{
    public int maxHP = 7;
    public int hp;

    public HealState healState;
    public FleeState fleeState;
    public ChaseState chaseState;
    public AttackState attackState;
    public SummonState summonState;

    public Pool<EnemyBullet> bulletPool;
    
    [HideInInspector] public PlayerModel player;
    
    private FiniteStateMachine _fsm;

    private float _lastReplanTime;
    private float _replanRate = .5f;
    
    private void Start() 
    {
        hp = maxHP;

        chaseState.OnNeedsReplan += OnReplan;
        attackState.OnNeedsReplan += OnReplan;
        
        player = FindObjectOfType<PlayerModel>();
        
        EnemyBulletBuilder bulletBuilder = new EnemyBulletBuilder();
        bulletBuilder.Configure(attackState.bulletSpeed, attackState.timeToDestroy);
        bulletPool = new Pool<EnemyBullet>(bulletBuilder.Build, EnemyBullet.TurnOn, EnemyBullet.TurnOff, 5);
        
        //OnlyPlan();
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

            new GOAPAction("Heal")
                .Pre("isLowHP",false)
                .Effect("isRegenerating", true)
                .LinkedState(healState),

            new GOAPAction("Summon")
                .Pre("areLowAsteroids",false)
                .Effect("isPlayerAlive",false)
                .LinkedState(summonState),

            new GOAPAction("Flee")
                .Pre("isPlayerTooNear",false)
                .Effect("isPlayerAlive",true)
                .LinkedState(fleeState),
        };
        
        var from = new GOAPState();
        from.values["isPlayerNear"] = false;
        from.values["isPlayerAlive"] = true;
        from.values["isPlayerTooNear"] = false;

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
                .Effect("isPlayerNear",true)
                .LinkedState(chaseState),

            new GOAPAction("Attack")
                .Pre("isPlayerNear",true)
                .Effect("isPlayerAlive",false)
                .LinkedState(attackState),

            new GOAPAction("Heal")
                .Pre("isLowHP",false)
                .Effect("isRegenerating", true)
                .LinkedState(healState),

            new GOAPAction("Summon")
                .Pre("areLowAsteroids",false)
                .Effect("isPlayerAlive",false)
                .LinkedState(summonState),

            new GOAPAction("Flee")
                .Pre("isPlayerTooNear",false)
                .Effect("isPlayerAlive",true)
                .LinkedState(fleeState),
        };

        var from = new GOAPState();
        from.values["isPlayerNear"] = false;
        from.values["isPlayerAlive"] = true;
        from.values["isPlayerTooNear"] = false;

        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;

        var planner = new GOAPPlanner();

        var plan = planner.Run(from, to, actions);
        
        ConfigureFsm(plan);
    }

    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        _fsm.Active = false;
        //Debug.Log("Completed Plan");
        _fsm = GOAPPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }
}
