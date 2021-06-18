using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : IWeapon
{
    private PlayerModel _playerModel;
    private PlayerController _playerController;
    
    float fireRate = 0.2f;

    public void GetPlayerInput(PlayerModel playerModel, PlayerController playerController)
    {
        this._playerModel = playerModel;
        _playerController = playerController;
    }

    public void Shoot()
    {
        var bullet = _playerController.bulletPool.Get();

        bullet.pool = _playerController.bulletPool;
        bullet.transform.position = _playerModel.spawnPoint.position;
        bullet.transform.eulerAngles = _playerModel.transform.eulerAngles;

        _playerController.currentFireRate = fireRate;
    }
}