using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : IWeapon
{
    PlayerInput _playerInput;
    float fireRate;

    public void GetPlayerInput(PlayerInput playerInput)
    {
        _playerInput = playerInput;
    }

    public void Shoot()
    {
        //piuum piuum laser sounds

        _playerInput.currentFireRate = fireRate;
    }
}
