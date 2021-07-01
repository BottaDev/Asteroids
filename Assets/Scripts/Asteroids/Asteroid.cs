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
        EventManager.Instance.Subscribe("OnSave", SaveAsteroid);
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe("OnGameFinished", OnGameFinished);
        EventManager.Instance.Unsubscribe("OnSave", SaveAsteroid);
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

    private void SaveAsteroid(params object[] parameters)
    {
        print("Asteroid saved");
        SavestateManager.Instance.saveState.asteroidList.Add(new AsteroidData(this));
    }
}



[Serializable]
public class AsteroidData
{
    public float x;
    public float y;
    public float z;
    public float zRotation;

    public AsteroidData(Asteroid asteroid)
    {
        x = asteroid.transform.position.x;
        y = asteroid.transform.position.y;
        z = asteroid.transform.position.z;
        zRotation = asteroid.transform.eulerAngles.z;
    }
}
