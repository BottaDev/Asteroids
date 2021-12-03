using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : MonoBaseState
{
    public float speed;
    public float nearDistance = 1.5f;

    private Vector3 _velocity;
    private EliteEnemy _enemy;
    private float _playerDistance = 999f;
    public override event Action OnNeedsReplan;
    
    private void Awake()
    {
        _enemy = GetComponent<EliteEnemy>();
    }

    public override void UpdateLoop()
    {
        _playerDistance = Vector3.Distance(transform.position, _enemy.player.transform.position);
        if (_playerDistance < nearDistance)
            MoveFlee();
    }

    private void MoveFlee()
    {
        transform.position = Vector2.MoveTowards(transform.position, _enemy.player.transform.position, -speed * Time.deltaTime);
    }

    public override IGoapState ProcessInput()
    {
        if (_playerDistance > nearDistance)
        {
            if (!Transitions.ContainsKey("OnAttackState"))
            {
                OnNeedsReplan?.Invoke();
                return this;
            }
            
            return Transitions["OnAttackState"];
        }
        
        if (_enemy.currentHp <= _enemy.maxHP / 3)
        {
            if (!Transitions.ContainsKey("OnHealState"))
            {
                OnNeedsReplan?.Invoke();
                return this;
            }
            
            return Transitions["OnHealState"];
        }

        return this;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, nearDistance);
    }
}