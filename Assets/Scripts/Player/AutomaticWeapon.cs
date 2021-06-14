using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : IWeapon
{
    public PlayerController playerController;
    float fireRate = 0.2f;

    public void GetPlayerInput(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Shoot()
    {
        var bullet = playerController.bulletPool.Get();

        bullet.pool = playerController.bulletPool;
        bullet.transform.position = playerController.spawnPoint.position;
        bullet.transform.eulerAngles = playerController.transform.eulerAngles;

        playerController.currentFireRate = fireRate;
    }
}