using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Animations;

public class Asteroid : Entity
{   
    public Pool<Asteroid> pool;

    private void Start()
    {
        EventManager.Instance.Subscribe("OnGameFinished", OnGameFinished);
    }

    public void Configure(float speed) 
    {
        AsteroidFlyweightPoint.normal.speed = speed;
    }
	
    protected void Update()
    {
        base.Update();
        
        transform.position += transform.up * (AsteroidFlyweightPoint.normal.speed * Time.deltaTime);
    }
    
    public static void TurnOn(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(true);
    }
    
    public static void TurnOff(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(false);
    }

    public void HitByLaser()
    {
        DestroyAsteroid();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player bullet
        if (other.gameObject.layer == 8)
            DestroyAsteroid();
    }
    
    private void DestroyAsteroid(bool hasScore = true)
    {
        if(hasScore)
            EventManager.Instance.Trigger("OnAsteroidDestroyed", AsteroidFlyweightPoint.normal.points);
        pool.ReturnToPool(this);
    }

    private void OnGameFinished(params object[] parameters)
    {
        DestroyAsteroid(false);
    }
}
