using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy : Entity, IReminder
{
    public int maxHP = 12;
    public int currentHp;

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
    private Memento<ObjectSnapshot> _memento = new Memento<ObjectSnapshot>();

    public float nearDistance;
    public float attackDistance;
    
    protected override void Start()
    {
        base.Start();
        
        currentHp = maxHP;

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
                .Effect("isPlayerAlive",false)
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
        
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < attackDistance)
        {
            // AttackState
            from.values["isPlayerNear"] = true;
            
            // FleeState
            if (distance < nearDistance)
                from.values["isPlayerTooNear"] = true;
            else
                from.values["isPlayerTooNear"] = false;
        }
        else
        {
            from.values["isPlayerNear"] = false;        // AttackState && ChaseState
            from.values["isPlayerTooNear"] = false;     // FleeState
        }

        from.values["areLowAsteroids"] = true;
        if(Time.time >= 5)
            from.values["areLowAsteroids"] = false; 

        // HealState
        if (currentHp <= maxHP / 3)
            from.values["isLowHP"] = true;
        else
            from.values["isLowHP"] = false;

        //Debug.Log("IsLowHP: " + from.values["isLowHP"]);

        var to = new GOAPState();
        to.values["isLowHP"] = false;
        to.values["isPlayerTooNear"] = false;
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
        currentHp--;
        if (currentHp <=0)
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
    
    void Die()
    {
        Destroy(gameObject);
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
}
