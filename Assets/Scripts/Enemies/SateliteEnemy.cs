using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//IA2-P1
public class SateliteEnemy : Entity
{
    public float speed;
    public float searchRadius;

    public float fastRotationSpeed;
    public float slowRotationSpeed;
    public Transform sprite;
    public Transform radiusSprite;
    public Pool<SateliteEnemy> pool;

    [HideInInspector] public float currentRotationSpeed;
    [HideInInspector] public CircleQuery query;
    [HideInInspector] public PlayerModel target;

    private StateMachine _sm;


    public void Configure(float _speed)
    {
        speed = _speed;
    }


    private void Awake()
    {
        query = GetComponent<CircleQuery>();
        query.radius = searchRadius;
        if (sprite == null) sprite = GetComponentInChildren<SpriteRenderer>().transform;
        if (radiusSprite == null) radiusSprite = GetComponentsInChildren<SpriteRenderer>()[1].transform;

        radiusSprite.localScale = Vector3.one * searchRadius * 2.25f;

        _sm = new StateMachine();
        _sm.AddState("Wander", new SateliteWanderState(_sm, this));
        _sm.AddState("Chase", new SateliteChaseState(_sm, this));

        _sm.ChangeState("Wander");
    }

    protected void Update()
    {
        base.Update();
        _sm.OnUpdate();

        sprite.eulerAngles += Vector3.forward * currentRotationSpeed;
        transform.position += transform.up * speed * Time.deltaTime;
    }

    public void HitByLaser()
    {
        Die();
    }

    public void HitByBomb()
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

    public static void TurnOn(SateliteEnemy satelite)
    {
        satelite.gameObject.SetActive(true);
    }

    public static void TurnOff(SateliteEnemy satelite)
    {
        satelite.gameObject.SetActive(false);
    }
}

public class SateliteWanderState : IState
{
    private SateliteEnemy _satelite;
    private StateMachine _sm;

    public SateliteWanderState(StateMachine sm, SateliteEnemy satelite)
    {
        _satelite = satelite;
        _sm = sm;
    }

    public void OnEnter()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                _satelite.transform.eulerAngles = new Vector3(0, 0, 45);
                break;
            case 1:
                _satelite.transform.eulerAngles = new Vector3(0, 0, 135);
                break;
            case 2:
                _satelite.transform.eulerAngles = new Vector3(0, 0, 225);
                break;
            case 3:
                _satelite.transform.eulerAngles = new Vector3(0, 0, 315);
                break;
        }

        _satelite.currentRotationSpeed = _satelite.slowRotationSpeed;

        Debug.Log("Wander - OnEnter");
    }

    public void OnExit()
    {
        Debug.Log("Wander - OnExit");
    }

    public void OnUpdate()
    {
        
        if (_satelite.target != null)
        {
            _sm.ChangeState("Chase");
        }

        List<PlayerModel> pm = _satelite.query.Query()
            .OfType<PlayerModel>()
            .ToList<PlayerModel>();
        if (pm.Count > 0)
            _satelite.target = pm[0];
       
    }
}

public class SateliteChaseState : IState
{
    private SateliteEnemy _satelite;
    private StateMachine _sm;

    public SateliteChaseState(StateMachine sm, SateliteEnemy satelite)
    {
        _satelite = satelite;
        _sm = sm;
    }

    public void OnEnter()
    {
        Debug.Log("Chase - OnEnter");
        if (_satelite.target == null)
            _satelite.target = GameObject.Find("Player").GetComponent <PlayerModel>();

        _satelite.currentRotationSpeed = _satelite.fastRotationSpeed;
    }

    public void OnExit()
    {
        Debug.Log("Chase - OnExit");
    }

    public void OnUpdate()
    {
        _satelite.transform.up = (Vector2)(_satelite.target.transform.position - _satelite.transform.position).normalized;
    }
}