using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEnemy : Entity
{
    private StateMachine _sm;
    public Pool<ShipEnemy> pool;

    [HideInInspector] public LineRenderer lineRenderer;
    [HideInInspector] public PlayerController target;
    [HideInInspector] public Vector2 targetPoint;
    public float currentSpeed;
    
    [Range(0f, 2f)]   public float patrolArea;
    public float speed;
    public float overshoot;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        _sm = new StateMachine();
        _sm.AddState("Patrol", new ShipPatrolState(_sm, this));
        _sm.AddState("Attack", new ShipAttackState(_sm, this));

        _sm.ChangeState("Patrol");
    }

    protected void Update()
    {
        base.Update();
        _sm.OnUpdate();

        transform.position += transform.up * currentSpeed * Time.deltaTime;
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

    public static void TurnOn(ShipEnemy ship)
    {
        ship.gameObject.SetActive(true);
    }

    public static void TurnOff(ShipEnemy ship)
    {
        ship.gameObject.SetActive(false);
    }
}

public class ShipPatrolState : IState
{
    private StateMachine _sm;
    private ShipEnemy _ship;

    public ShipPatrolState(StateMachine sm, ShipEnemy ship)
    {
        _sm = sm;
        _ship = ship;
    }

    public void OnEnter()
    {
        Debug.Log("Patrol - OnEnter");

        _ship.currentSpeed = _ship.speed;
    }

    public void OnExit()
    {
        Debug.Log("Patrol - OnExit");
    }

    public void OnUpdate()
    {
        _ship.transform.eulerAngles += Vector3.forward * (1 + _ship.patrolArea);

        RaycastHit2D ray = Physics2D.Raycast(_ship.transform.position + _ship.transform.up * 0.5f, _ship.transform.up);
        if (ray.collider != null && ray.collider.gameObject.layer == 10)
        {
            _ship.targetPoint = ray.collider.transform.position + (ray.collider.transform.position - _ship.transform.position).normalized * _ship.overshoot;
            _sm.ChangeState("Attack");
        }
    }
}

public class ShipAttackState : IState
{
    private StateMachine _sm;
    private ShipEnemy _ship;

    public ShipAttackState(StateMachine sm, ShipEnemy ship)
    {
        _sm = sm;
        _ship = ship;
    }

    public void OnEnter()
    {
        _ship.lineRenderer.SetPosition(0, _ship.transform.position);
        _ship.lineRenderer.SetPosition(1, _ship.targetPoint);

        _ship.transform.up = ((Vector3)_ship.targetPoint - _ship.transform.position).normalized;
        _ship.currentSpeed = 0.01f;

        Debug.Log("Attack - OnEnter");
    }

    public void OnExit()
    {
        _ship.lineRenderer.SetPosition(0, Vector3.zero);
        _ship.lineRenderer.SetPosition(1, Vector3.zero);

        Debug.Log("Attack - OnExit");
    }

    public void OnUpdate()
    {
        Vector3 AB = (Vector3)_ship.targetPoint - _ship.lineRenderer.GetPosition(0);
        Vector3 AV = _ship.transform.position - _ship.lineRenderer.GetPosition(0);
        float t = Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);

        _ship.currentSpeed = _ship.speed * 8 * BezierSpeed(new Vector2(0,0.01f), new Vector2(1.1f, 0.9f), new Vector2(0.1f, 1), new Vector2(1, 0.01f), t);

        if (t >= 1 || t < 0)
        {
            _sm.ChangeState("Patrol");
        }
    }

    float BezierSpeed(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t) //receives a point between start and destination and returns the apropiate speed corresponding to a bezier curve
    {
        Vector2 A = Vector2.Lerp(p0, p1, t);
        Vector2 B = Vector2.Lerp(p1, p2, t);
        Vector2 C = Vector2.Lerp(p2, p3, t);
        Vector2 D = Vector2.Lerp(A, B, t);
        Vector2 E = Vector2.Lerp(B, C, t);
        Vector2 P = Vector2.Lerp(D, E, t);

        return P.y;
    }
}