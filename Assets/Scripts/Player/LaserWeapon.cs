using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : IWeapon
{
    private PlayerModel _playerModel;
    private PlayerController _playerController;
    
    private bool _isLaserOn = false;
    private float fireRate = 5; 
    private float duration = 1.5f;
    private float range = 5;
    private LineRenderer _lr;

    public void GetPlayerInput(PlayerModel playerModel, PlayerController playerController)
    {
        _playerModel = playerModel;
        _playerController = playerController;
        _lr = playerModel.spawnPoint.GetComponent<LineRenderer>();
    }

    public void Shoot()
    {
        _playerController.currentFireRate = fireRate;

        if (!_isLaserOn)
        {
            _playerModel.StartCoroutine(Laser());
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

            if (hit = Physics2D.Raycast(_playerModel.spawnPoint.position, _playerModel.transform.up, range, layerMask))
            {
                Debug.Log(hit.collider.name);
                _lr.SetPosition(1, hit.point);
                hit.collider.GetComponent<Asteroid>().HitByLaser();
            }
            else
            {
                Debug.Log("he does miss sometimes :((");
                _lr.SetPosition(1, _playerModel.spawnPoint.position + _playerModel.transform.up * range);
            }

            yield return new WaitForSeconds(1/60);
        }

        _isLaserOn = false;
        _lr.enabled = false;
    }
}
