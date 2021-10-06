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
    LayerMask layerMask = (1 << 9);
    IEnumerator Laser()
    {
        _lr.enabled = true;
        _isLaserOn = true;

        for (int i = 0; i < duration*60; i++)
        {
            _lr.SetPosition(0, _playerModel.spawnPoint.position);

            if (hit = Physics2D.Raycast(_playerModel.spawnPoint.position, _playerController.transform.up, range, layerMask))
            {
                Debug.Log(hit.collider.name);
                _lr.SetPosition(1, hit.point);
                hit.collider.GetComponent<Asteroid>().HitByLaser();
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
