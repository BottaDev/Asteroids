using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonState : MonoBaseState
{
    float summonInterval = 1.75f;
    float next;

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
    }

    void Summon()
    {
        var asteroid = _asteroidPool.Get();
        asteroid.pool = _asteroidPool;
        asteroid.transform.position = transform.position;

        Vector3 diff = _enemy.player.transform.position - asteroid.transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        asteroid.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    public override IGoapState ProcessInput()
    {
        if (_enemy.hp <= 3 && Transitions.ContainsKey("OnHealState"))
            return Transitions["OnHealState"];

        return this;
    }

}
