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
    }

    private void MoveFlee()
    {
        transform.position = Vector2.MoveTowards(transform.position, _enemy.player.transform.position, -speed * Time.deltaTime);
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
        
        /*if (_playerDistance > _enemy.attackDistance)
        {
            if (!Transitions.ContainsKey("OnSummonState"))
            {
                OnNeedsReplan?.Invoke();
                return this;
            }
            return Transitions["OnSummonState"];
        }*/

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