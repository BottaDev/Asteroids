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

    private void Awake()
    {
        _enemy = GetComponent<EliteEnemy>();
    }

    public override void UpdateLoop()
    {
        float distance = Vector3.Distance(transform.position, _enemy.player.transform.position);
        if (distance <= nearDistance)
            MoveFlee();
    }

    private void MoveFlee()
    {
        transform.position = Vector2.MoveTowards(transform.position, _enemy.player.transform.position, -speed * Time.deltaTime);
    }

    public override IGoapState ProcessInput()
    {
        float distance = Vector2.Distance(transform.position, _enemy.player.transform.position);

        if (distance >= nearDistance && Transitions.ContainsKey("OnAttackState"))
            return Transitions["OnAttackState"];

        if (_enemy.hp <= 3 && Transitions.ContainsKey("OnHealState"))
            return Transitions["OnHealState"];

        return this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, nearDistance);
    }
}