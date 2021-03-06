using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonState : MonoBaseState
{
    public int minAsteroids = 2;
    
    private float _summonInterval = 2f;
    private float _next;
    private EliteEnemy _enemy;
    public override event Action OnNeedsReplan;
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
        if (Time.time >= _next)
        {
            _next = Time.time + _summonInterval;
            Summon();
        }
        
        RotateTowardsPlayer();
    }

    private void RotateTowardsPlayer()
    {
        float offset = 270f;
        Vector2 direction = _enemy.player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;       
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
    
    private void Summon()
    {
        var asteroid = _asteroidPool.Get();
        asteroid.pool = _asteroidPool;
        asteroid.transform.position = transform.position;

        asteroid.transform.rotation = transform.rotation;
    }

    public override IGoapState ProcessInput()
    {
		if (_enemy.currentHp <= _enemy.maxHP / 3)
        {
            if (!Transitions.ContainsKey("OnHealState"))
            {
                OnNeedsReplan?.Invoke();
                return this;
            }
            
            return Transitions["OnHealState"];
        } 
		
        float distance = Vector2.Distance(transform.position, _enemy.player.transform.position);

        Debug.Log("SummonState Process Input. \nPlayer outside of range: " + (distance > _enemy.attackDistance) + "\nCurrent HP: " + _enemy.currentHp + "\nElements in 'Transitions': " + Transitions.Count);

        if (_enemy.attackDistance <= distance)
        {
            if (!Transitions.ContainsKey("OnAttackState"))
            {
                OnNeedsReplan?.Invoke();
                return this;
            }
            
            return Transitions["OnAttackState"];
        }
		else if (_enemy.attackDistance > distance)
        {
            if (!Transitions.ContainsKey("OnChaseState"))
            {
                OnNeedsReplan?.Invoke();
                return this;
            }
			
			return Transitions["OnChaseState"];
        }

        return this;
    }

}
