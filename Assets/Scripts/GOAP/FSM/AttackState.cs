using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBaseState
{
    private EliteEnemy _enemy;
    
    private void Awake()
    {
        _enemy = GetComponent<EliteEnemy>();
    }
    
    public override void UpdateLoop()
    {
        RotateTowardsPlayer();
        Shoot();
    }

    private void Shoot()
    {
        
    }
    
    private void RotateTowardsPlayer()
    {
        float offset = 270f;
        Vector2 direction = _enemy.player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;       
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
    
    public override IGoapState ProcessInput() { return this; }
}
