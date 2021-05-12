using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
