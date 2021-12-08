using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class AttackState : MonoBaseState
{
    public float fireRate = 0.5f;
    public float bulletSpeed = 7f;
    public float timeToDestroy = 5f;
    public Transform spawnPoint;
    
    private float _currentFireRate;
    private EliteEnemy _enemy;
    public override event Action OnNeedsReplan;
    
    private void Awake()
    {
        _enemy = GetComponent<EliteEnemy>();
    }
    
    public override void UpdateLoop()
    {
        RotateTowardsPlayer();
        
        if (_currentFireRate <= 0)
            Shoot();
        else
            _currentFireRate -= Time.deltaTime;
    }

    private void Shoot()
    {
        var bullet = _enemy.bulletPool.Get();

        bullet.pool = _enemy.bulletPool;
        bullet.transform.position = spawnPoint.position;
        bullet.transform.eulerAngles = _enemy.transform.eulerAngles;

        _currentFireRate = fireRate;
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
     
        if (_enemy.currentHp <= _enemy.maxHP / 3)
        {
            if (!Transitions.ContainsKey("OnHealState"))
            {
                OnNeedsReplan?.Invoke();
                return this;
            }
            
            return Transitions["OnHealState"];
        } 
        
        if (distance > _enemy.attackDistance)
        {
            if (!Transitions.ContainsKey("OnChaseState"))
            {
                OnNeedsReplan?.Invoke();
                return this;        
            }

            return Transitions["OnChaseState"];    
        }


        if (distance < _enemy.nearDistance)
        {
            if (!Transitions.ContainsKey("OnFleeState"))
            {
                OnNeedsReplan?.Invoke();
                return this;
            }
            
            return Transitions["OnFleeState"];
        }

        return this;
    }
}
