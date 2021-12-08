﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : MonoBaseState
{
    public float speed = 4f;
    
    private EliteEnemy _enemy;
    
    private void Awake()
    {
        _enemy = GetComponent<EliteEnemy>();
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
        float distance = Vector2.Distance(transform.position, _enemy.player.transform.position);

        if (distance <= _enemy.attackDistance && Transitions.ContainsKey("OnAttackState"))
            return Transitions["OnAttackState"];

        return this;
    }

    public override void Enter(IGoapState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemy.attackDistance);
    }
}
