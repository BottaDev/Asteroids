using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : IWeapon
{
    private PlayerController _playerController;
    private PlayerModel _playerModel;
    float fireRate = 0.2f;

    public void GetPlayerInput(PlayerModel playerModel, PlayerController playerController)
    {
        this._playerController = playerController;
        this._playerModel = playerModel;
    }

    public void Shoot()
    {
        var bullet = _playerController.bulletPool.Get();

        bullet.pool = _playerController.bulletPool;
        bullet.transform.position = _playerModel.spawnPoint.position;
        bullet.transform.eulerAngles = _playerController.transform.eulerAngles;

        _playerModel.currentFireRate = fireRate;
    }
}