using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Transform spawnPoint;
    public Pool<Bullet> bulletPool;
    public float currentFireRate;

    private Player _player;
    
    private void Awake()
    {
        _player = GetComponent<Player>();
        
        BulletFactory factory = new BulletFactory();
        bulletPool = new Pool<Bullet>(factory.Create, Bullet.TurnOn, Bullet.TurnOff, 5);
        
        currentFireRate = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentFireRate <= 0)
            _player.weapons[_player.currentWeaponIndex].Shoot();
            //Shoot();
        else if (Input.GetKeyDown(KeyCode.E))
            _player.NextWeapon();
        else
            currentFireRate -= Time.deltaTime;
    }

    /*
    private void Shoot()
    {
        var bullet = _bulletPool.Get();

        bullet.pool = _bulletPool;
        bullet.transform.position = spawnPoint.position;
        bullet.transform.forward = transform.forward;

        _currentFireRate = _player.FireRate;
    }
    */
}
