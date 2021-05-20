using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : IWeapon
{
    PlayerInput _playerInput;
    bool _isLaserOn = false;

    float fireRate = 5; 
    float duration = 1.5f;
    float range = 5;


    LineRenderer _lr;

    public void GetPlayerInput(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        _lr = _playerInput.spawnPoint.GetComponent<LineRenderer>();
    }

    public void Shoot()
    {
        _playerInput.currentFireRate = fireRate;

        if (!_isLaserOn)
        {
            _playerInput.StartCoroutine(Laser());
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
            _lr.SetPosition(0, _playerInput.spawnPoint.position);

            if (hit = Physics2D.Raycast(_playerInput.spawnPoint.position, _playerInput.transform.up, range, layerMask))
            {
                Debug.Log(hit.collider.name);
                _lr.SetPosition(1, hit.point);
                hit.collider.GetComponent<Asteroid>().HitByLaser();
            }
            else
            {
                Debug.Log("he does miss sometimes :((");
                _lr.SetPosition(1, _playerInput.spawnPoint.position + _playerInput.transform.up * range);
            }

            yield return new WaitForSeconds(1/60);
        }

        _isLaserOn = false;
        _lr.enabled = false;
    }
}
