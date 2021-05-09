using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Transform spawnPoint;
    
    private Player _player;
    public float _currentFireRate;
    private Pool<Bullet> _bulletPool;
    
    private void Awake()
    {
        _player = GetComponent<Player>();
        
        BulletFactory factory = new BulletFactory();
        _bulletPool = new Pool<Bullet>(factory.Create, Bullet.TurnOn, Bullet.TurnOff, 5);
        
        _currentFireRate = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _currentFireRate <= 0)
            Shoot();
        else
            _currentFireRate -= Time.deltaTime;
    }

    private void Shoot()
    {
        var bullet = _bulletPool.Get();

        bullet.pool = _bulletPool;
        bullet.transform.position = spawnPoint.position;
        bullet.transform.forward = transform.forward;

        _currentFireRate = _player.FireRate;
    }
}
