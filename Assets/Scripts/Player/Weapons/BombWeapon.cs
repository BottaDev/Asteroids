using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MyA1-P3
public class BombWeapon : IWeapon
{
    private PlayerController _playerController;
    private PlayerModel _playerModel;
    private float _fireRate = 2f; 
    
    // Place the bomb
    public void Shoot()
    {
        if (_playerController.exploding)
            return;
        
        var bomb = _playerController.bombPool.Get();

        bomb.pool = _playerController.bombPool;
        bomb.transform.position = _playerModel.transform.position;

        _playerModel.currentFireRate = _fireRate;
        
        _playerController.activeBombs.Add(bomb);
    }

    public void GetPlayerInput(PlayerModel playerModel, PlayerController playerController)
    {
        _playerController = playerController;
        _playerModel = playerModel;
    }
}
