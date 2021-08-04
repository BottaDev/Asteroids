using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PowerUp : MonoBehaviour, IPowerUp
{
    private float _timeToDestroy = 15f;
    
    public Pool<PowerUp> pool;
    public PowerUpType powerUpType;

    private IPowerUp current;


    protected void Start()
    {
        SetDecorator();
        StartCoroutine(DisableOnTime());
    }

    //MyA1-P4 punto 2-2
    private void SetDecorator()
    {
        if (powerUpType == PowerUpType.Heal)
        {
            current = new HealPowerUp(this);
        }
        else
        {
            current = new RewindPowerUp(this);
        }
    }
    
    public void Configure(float time)
    {
        _timeToDestroy = time;
    }
    
    public void UsePowerUp(PlayerController player)
    {
        pool.ReturnToPool(this); 
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
        if (other.gameObject.layer == 10)
            current.UsePowerUp(other.GetComponent<PlayerController>());
    }

    private IEnumerator DisableOnTime()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        
        if(gameObject.activeSelf)
            pool.ReturnToPool(this);
    }

    public enum PowerUpType
    {
        Heal,
        Rewind
    }
}
