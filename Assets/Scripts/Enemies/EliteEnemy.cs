using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EliteEnemy : Entity, IReminder
{
    public HealState healState;
    public FleeState fleeState;
    public ChaseState chaseState;
    public AttackState attackState;
    public SummonState summonState;
    
    public Pool<EliteEnemy> pool;

    private FiniteStateMachine _fsm;
    private float _lastReplanTime;
    private float _replanRate = .5f;
    private IQuery _query;
    private EliteEnemyState _enemyState;
    private Memento<ObjectSnapshot> _memento = new Memento<ObjectSnapshot>();

    private void Awake()
    {
        _query = GetComponent<IQuery>();
    }
    
    protected override void Start()
    {
        base.Start();

        _enemyState = GetComponent<EliteEnemyState>();

        chaseState.OnNeedsReplan += OnReplan;
        attackState.OnNeedsReplan += OnReplan;
        healState.OnNeedsReplan += OnReplan;
        fleeState.OnNeedsReplan += OnReplan;
        summonState.OnNeedsReplan += OnReplan;

        PlanAndExecute();
    }

    private void PlanAndExecute() 
    {
        var actions = new List<GOAPAction>
        {
            new GOAPAction("Chase")
                .Pre("isPlayerNear",false)
                .Effect("isPlayerNear",true)
                .LinkedState(chaseState),

            new GOAPAction("Attack")
                .Pre("isPlayerNear",true)
                .Effect("isPlayerAlive",false)
                .LinkedState(attackState),

            new GOAPAction("Heal")
                .Pre("isLowHP",true)
                .Pre("isPlayerTooNear",false)
                .Effect("isLowHP",false)
                .LinkedState(healState),

            new GOAPAction("Summon")
                .Pre("areLowAsteroids",true)
                .Pre("isPlayerNear",false)
                .Pre("isPlayerTooNear",false)
                .Effect("areLowAsteroids",false)
                .LinkedState(summonState),

            new GOAPAction("Flee")
                .Pre("isPlayerNear",true)
                .Pre("isPlayerTooNear",true)
                .Effect("isPlayerNear",false)
                .Effect("isPlayerTooNear",false)
                .LinkedState(fleeState),
        };
        
        var from = new GOAPState();
        
        float distance = Vector2.Distance(transform.position, _enemyState.player.transform.position);
        if (distance <= _enemyState.attackDistance)
        {
            // AttackState
            from.values["isPlayerNear"] = true;
            
            // FleeState
            if (distance <= _enemyState.nearDistance)
                from.values["isPlayerTooNear"] = true;
            else
                from.values["isPlayerTooNear"] = false;
        }
        else
        {
            from.values["isPlayerNear"] = false;        // AttackState && ChaseState
            from.values["isPlayerTooNear"] = false;     // FleeState
        }

        // HealState
        if (_enemyState.currentHp <= _enemyState.maxHp / 3)
            from.values["isLowHP"] = true;
        else
            from.values["isLowHP"] = false;
        
        int asteroids = _query.Query()
            .OfType<Asteroid>()
            .Where(x => x.enabled)
            .ToList().Count;
        
        // SummonState
        if (asteroids <= summonState.minAsteroids)
            from.values["areLowAsteroids"] = true;
        else
            from.values["areLowAsteroids"] = false;

        var to = new GOAPState();
        to.values["isLowHP"] = false;
        to.values["isPlayerTooNear"] = false;
        to.values["areLowAsteroids"] = false;
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
        
        if (_fsm != null)
            _fsm.Active = false;
        _fsm = GOAPPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

    private void TakeDamage()
    {
        _enemyState.currentHp--;
        if (_enemyState.currentHp <=0)
            Die();
    }
    
    public override void HitByLaser()
    {
        TakeDamage();
    }

    public override void HitByBomb()
    {
        TakeDamage();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player bullet
        if (other.gameObject.layer == 8)
            TakeDamage();
    }

    public void MakeSnapshot()
    {
        var snapshot = new ObjectSnapshot();
        snapshot.position = transform.position;
        snapshot.rotation = transform.localRotation;
        
        _memento.Record(snapshot);
    }

    public void Rewind()
    {
        if (!_memento.CanRemember()) 
            return;
        
        var snapshot = _memento.Remember();

        transform.position = snapshot.position;
        transform.rotation = snapshot.rotation;
    }

    public IEnumerator StartToRecord()
    {
        while (true) 
        {
            if (gameObject.activeSelf)
                MakeSnapshot();
            
            yield return new WaitForSeconds(.1f);
        }
    }
    
    public static void TurnOn(EliteEnemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    public static void TurnOff(EliteEnemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
    
    void Die(bool hasScore = true)
    {
        if (hasScore)
            EventManager.Instance.Trigger("OnAsteroidDestroyed", AsteroidFlyweightPoint.normal.points);
        
        pool.ReturnToPool(this);
    }
}
