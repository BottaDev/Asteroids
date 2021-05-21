using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : IWeapon
{
    public PlayerInput _playerInput;
    float fireRate = 0.2f;

    public void GetPlayerInput(PlayerInput playerInput)
    {
        _playerInput = playerInput;
    }

    public void Shoot()
    {
        var bullet = _playerInput.bulletPool.Get();

        bullet.pool = _playerInput.bulletPool;
        bullet.transform.position = _playerInput.spawnPoint.position;
        bullet.transform.eulerAngles = _playerInput.transform.eulerAngles;

        _playerInput.currentFireRate = fireRate;
    }
}