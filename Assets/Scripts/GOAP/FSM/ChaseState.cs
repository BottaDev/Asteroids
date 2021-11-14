using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : MonoBaseState
{
    public float speed = 4f;
    
    private PlayerModel _player;
    
    private void Awake()
    {
        _player = FindObjectOfType<PlayerModel>();
    }

    private void RotateTowardsPlayer()
    {
        if (_player == null)
            return;
        
        float offset = 90f;
        Vector2 direction = _player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;       
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
    
    public override void UpdateLoop()
    {
        RotateTowardsPlayer();
    }

    public override IGoapState ProcessInput()
    {
        throw new System.NotImplementedException();
    }
}
