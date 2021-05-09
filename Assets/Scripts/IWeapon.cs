using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Shoot();
}

public class AutomaticWeapon : IWeapon
{
    PlayerInput _playerInput;
    float fireRate;

    public void Shoot()
    {
        var bullet = _playerInput.bulletPool.Get();

        bullet.pool = _playerInput.bulletPool;
        bullet.transform.position = _playerInput.spawnPoint.position;
        bullet.transform.forward = _playerInput.transform.forward;

        _playerInput.currentFireRate = fireRate;
    }
}

public class LaserWeapon : IWeapon
{
    PlayerInput _playerInput;
    float fireRate;

    public void Shoot()
    {
        //piuum piuum laser sounds

        _playerInput.currentFireRate = fireRate;
    }
}
