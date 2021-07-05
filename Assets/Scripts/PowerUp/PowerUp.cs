using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour, IPowerUp
{
    public float timeToDestroy = 15f;
    
    protected void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
    
    public void UsePowerUp()
    {
        Destroy(gameObject);    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player
        if (other.gameObject.layer == 10)
            Destroy(gameObject);
    }
}
