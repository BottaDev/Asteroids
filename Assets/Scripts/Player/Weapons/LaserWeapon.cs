using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : IWeapon
{
    private PlayerController _playerController;
    private PlayerModel _playerModel;
    bool _isLaserOn = false;

    float fireRate = 5; 
    float duration = 1.5f;
    float range = 5;


    LineRenderer _lr;

    public void GetPlayerInput(PlayerModel playerModel, PlayerController playerController)
    {
        _playerController = playerController;
        _playerModel = playerModel;
        _lr = _playerModel.spawnPoint.GetComponent<LineRenderer>();
    }

    public void Shoot()
    {
        _playerModel.currentFireRate = fireRate;

        if (!_isLaserOn)
        {
            _playerController.StartCoroutine(Laser());
        }
    }


    RaycastHit2D hit;
    LayerMask asteroidLayerMask = (1 << 9);
    LayerMask enemyLayerMask = (1 << 13);
    IEnumerator Laser()
    {
        _lr.enabled = true;
        _isLaserOn = true;
        LayerMask finalLayerMask = asteroidLayerMask | enemyLayerMask;

        for (int i = 0; i < duration*60; i++)
        {
            _lr.SetPosition(0, _playerModel.spawnPoint.position);

            if (hit = Physics2D.Raycast(_playerModel.spawnPoint.position, _playerController.transform.up, range, finalLayerMask))
            {
                Debug.Log(hit.collider.name);
                _lr.SetPosition(1, hit.point);
                
                if (hit.collider.gameObject.layer == 9)             // Asteroid
                    hit.collider.GetComponent<Asteroid>().HitByLaser();
                else if (hit.collider.gameObject.layer == 13)        // Enemy
                    hit.collider.GetComponent<Entity>().HitByLaser();
                
            }
            else
            {
                _lr.SetPosition(1, _playerModel.spawnPoint.position + _playerController.transform.up * range);
            }

            yield return new WaitForSeconds(1/60);
        }

        _isLaserOn = false;
        _lr.enabled = false;
    }
}
