using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour, IPowerUp
{
    private float _timeToDestroy = 15f;
    
    public Pool<PowerUp> pool;
    
    protected void Start()
    {
        Destroy(gameObject, _timeToDestroy);
    }
    
    public void Configure(float time)
    {
        _timeToDestroy = time;
    }
    
    public void UsePowerUp()
    {
        Destroy(gameObject);    
    }
    
    public static void TurnOn(PowerUp powerUp)
    {
        powerUp.gameObject.SetActive(true);
    }
    
    public static void TurnOff(PowerUp powerUp)
    {
        powerUp.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player
        if (other.gameObject.layer == 10)
            Destroy(gameObject);
    }
}
