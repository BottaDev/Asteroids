using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RECUPERATORIO TP Lucas Zinovoy
public class BlasterEnemy : Entity, IReminder
{
    private StateMachine _sm;
    public Pool<BlasterEnemy> pool;
    public Pool<EnemyBullet> bulletPool;

    [HideInInspector] public Transform target;
    public float currentSpeed;

    public float speed;

    private Memento<ObjectSnapshot> _memento = new Memento<ObjectSnapshot>();

    public void Configure(float _speed)
    {
        speed = _speed;
        currentSpeed = speed;


        if (transform.position.y > 0)
            transform.up = Vector3.down;
    }

    private void Awake()
    {
        EnemyBulletBuilder bulletBuilder = new EnemyBulletBuilder();
        bulletBuilder.Configure(3, 5);
        bulletPool = new Pool<EnemyBullet>(bulletBuilder.Build, EnemyBullet.TurnOn, EnemyBullet.TurnOff, 5);

        //lineRenderer = GetComponent<LineRenderer>();
        _sm = new StateMachine();
        _sm.AddState("Wander", new BlasterWanderState(_sm, this));
        _sm.AddState("Aim",    new BlasterAimState(_sm, this));

        _sm.ChangeState("Wander");


    }

    protected override void Start()
    {
        base.Start();

        MementoManager.instance.Add(this);
        EventManager.Instance.Subscribe("OnRewind", OnRewind);
    }

    protected void Update()
    {
        base.Update();
        _sm.OnUpdate();
    }


    public override void HitByLaser()
    {
        Die();
    }

    public override void HitByBomb()
    {
        Die();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player bullet
        if (other.gameObject.layer == 8)
            Die();
    }

    void Die(bool hasScore = true)
    {
        if (hasScore)
            EventManager.Instance.Trigger("OnAsteroidDestroyed", AsteroidFlyweightPoint.normal.points);
        pool.ReturnToPool(this);
    }

    public static void TurnOn(BlasterEnemy blaster)
    {
        blaster.gameObject.SetActive(true);
    }

    public static void TurnOff(BlasterEnemy blaster)
    {
        blaster.gameObject.SetActive(false);
    }

    private void OnRewind(params object[] parameters)
    {
        Rewind();
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

    public void MakeSnapshot()
    {
        var snapshot = new ObjectSnapshot();
        snapshot.position = transform.position;
        snapshot.rotation = transform.localRotation;

        _memento.Record(snapshot);
    }
}


public class BlasterWanderState : IState
{
    private StateMachine _sm;
    private BlasterEnemy _blaster;

    public BlasterWanderState(StateMachine sm, BlasterEnemy blaster)
    {
        _sm = sm;
        _blaster = blaster;
    }

    public void OnEnter()
    {
        //Debug.Log("Wander - OnEnter");
        _blaster.currentSpeed = _blaster.speed;
    }

    public void OnExit()
    {
        //Debug.Log("Patrol - OnExit");
    }

    public void OnUpdate()
    {
        _blaster.transform.position += _blaster.transform.right * _blaster.currentSpeed * Time.deltaTime;

        RaycastHit2D ray = Physics2D.Raycast(_blaster.transform.position + _blaster.transform.up * 0.5f, _blaster.transform.up);
        if (ray.collider != null && ray.collider.gameObject.layer == 10)
        {
            _blaster.target = ray.collider.transform;
            _sm.ChangeState("Aim");
        }
        
    }
}

public class BlasterAimState : IState
{
    private StateMachine _sm;
    private BlasterEnemy _blaster;
    private float aimDuration = 3.5f;
    private float shootingTime;

    public BlasterAimState(StateMachine sm, BlasterEnemy blaster)
    {
        _sm = sm;
        _blaster = blaster;
    }

    public void OnEnter()
    {
        //Debug.Log("Wander - OnEnter");
        _blaster.currentSpeed = _blaster.speed;
        shootingTime = Time.time + aimDuration;
    }

    public void OnExit()
    {
        //Debug.Log("Patrol - OnExit");
    }

    public void OnUpdate()
    {
        if(_blaster.target)
            _blaster.transform.up = _blaster.target.position - _blaster.transform.position;
        
        if (Time.time >= shootingTime)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        var bullet = _blaster.bulletPool.Get();

        bullet.pool = _blaster.bulletPool;
        bullet.transform.position = _blaster.transform.position;
        bullet.transform.eulerAngles = _blaster.transform.eulerAngles;

        _blaster.transform.up = -_blaster.transform.up;
        _sm.ChangeState("Wander");
    }
}