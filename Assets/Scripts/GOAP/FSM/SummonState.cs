using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonState : MonoBaseState
{
    float summonInterval = 1f;
    float next;
    float speed = 40;

    private EliteEnemy _enemy;
    private Pool<Asteroid> _asteroidPool;

    private void Awake()
    {
        AsteroidBuilder asteroidBuilder = new AsteroidBuilder();
        asteroidBuilder.SetSpeed(1.5f);

        _enemy = GetComponent<EliteEnemy>();
        _asteroidPool = new Pool<Asteroid>(asteroidBuilder.Build, Asteroid.TurnOn, Asteroid.TurnOff, 3);
    }

    public override void UpdateLoop()
    {
        if (Time.time >= next)
        {
            next = Time.time + summonInterval;
            Summon();
        }

        transform.eulerAngles += Vector3.forward * speed * Time.deltaTime;
    }

    void Summon()
    {
        var asteroid = _asteroidPool.Get();
        asteroid.pool = _asteroidPool;
        asteroid.transform.position = transform.position;

        asteroid.transform.rotation = transform.rotation;
    }

    public override IGoapState ProcessInput()
    {
        if (_enemy.hp <= 3 && Transitions.ContainsKey("OnHealState"))
            return Transitions["OnHealState"];

        return this;
    }

}
