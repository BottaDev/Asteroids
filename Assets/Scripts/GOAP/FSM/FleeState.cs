using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : MonoBaseState
{
    public float speed;

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
        if (_playerDistance < _enemy.attackDistance)
            MoveFlee();
        
        RotateTowardsPlayer();
    }

    private void MoveFlee()
    {
        transform.position = Vector2.MoveTowards(transform.position, _enemy.player.transform.position, -speed * Time.deltaTime);
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
        Debug.Log("FleeState Process Input. \nPlayer outside of range: " + (_playerDistance > _enemy.attackDistance) + "\nCurrent HP: " + _enemy.currentHp + "\nElements in 'Transitions': " + Transitions.Count);

        if (_playerDistance > _enemy.nearDistance)
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
}