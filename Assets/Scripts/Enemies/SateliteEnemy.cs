using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SateliteEnemy : Entity
{
    public float speed;
    public float searchRadius;

    public float fastRotationSpeed;
    public float slowRotationSpeed;
    public Transform sprite;

    [HideInInspector] public float currentRotationSpeed;
    [HideInInspector] public IQuery query;
    [HideInInspector] public PlayerController target;

    private StateMachine _sm;


    private void Awake()
    {
        if (sprite == null) sprite = GetComponentInChildren<SpriteRenderer>().transform;

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

        if (Input.GetKeyDown(KeyCode.G))
        {
            _sm.ChangeState("Chase");
        }

        /*
        List<PlayerController> pc = _satelite.query.Query()
       .OfType<PlayerController>()
       .Where(x => (_satelite.transform.position - x.transform.position).sqrMagnitude <= _satelite.searchRadius)
       .ToList<PlayerController>();
        if (pc.Count != 0) _satelite.target = pc[0];
        */
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
            _satelite.target = GameObject.Find("Player").GetComponent<PlayerController>();

        _satelite.currentRotationSpeed = _satelite.fastRotationSpeed;
    }

    public void OnExit()
    {
        Debug.Log("Chase - OnExit");
    }

    public void OnUpdate()
    {
        _satelite.transform.up = _satelite.target.transform.position - _satelite.transform.position;
    }
}