using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Animations;

public class Asteroid : MonoBehaviour
{
    public float Speed;
    
    public Pool<Asteroid> pool;

    private void Update()
    {
        transform.position += transform.up * (Speed * Time.deltaTime);
    }

    public static void TurnOn(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(true);
    }
    
    public static void TurnOff(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(false);
    }

    private void DestroyAsteroid()
    {
        pool.ReturnToPool(this);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player bullet
        if (other.gameObject.layer == 8)
            DestroyAsteroid();
    }
}
