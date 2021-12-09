using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChaseState : MonoBaseState
{
    public float speed = 4f;
    public override event Action OnNeedsReplan;
    
    private EliteEnemy _enemy;
    private SummonState _summonState;
    private IQuery _query; 
    
    private void Awake()
    {
        _enemy = GetComponent<EliteEnemy>();
        _summonState = GetComponent<SummonState>();
        _query = GetComponent<IQuery>();
    }

    public override void UpdateLoop()
    {
        if (_enemy.player == null)
            return;
        
        RotateTowardsPlayer();
        Move();
    }

    private void Move()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, _enemy.player.transform.position, speed * Time.deltaTime);
    }

    private void RotateTowardsPlayer()
    {
        float offset = 270f;
        Vector2 direction = _enemy.player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;       
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
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

        if (distance <= _enemy.attackDistance && Transitions.ContainsKey("OnAttackState"))
            return Transitions["OnAttackState"];

        int asteroids = _query.Query()
            .OfType<Asteroid>()
            .Where(x => x.enabled)
            .ToList().Count;
        
        if (asteroids <= _summonState.minAsteroids)
        {
            if (!Transitions.ContainsKey("OnSummonState"))
            {
                OnNeedsReplan?.Invoke();
                return this;
            }
            
            return Transitions["OnSummonState"];
        }
        
        return this;
    }
}
