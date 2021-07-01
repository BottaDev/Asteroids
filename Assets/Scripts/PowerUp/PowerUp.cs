using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float timeToDestroy;
    public Pool<PowerUp> pool;
    
    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    protected virtual void Use(PlayerModel playerModel)
    {
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
            Use(other.gameObject.GetComponent<PlayerModel>());
    }
}
