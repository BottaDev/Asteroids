using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Animations;

public class Asteroid : Entity
{
    public float Speed;
    public int points = 10;
    
    public Pool<Asteroid> pool;

    private void Start()
    {
        EventManager.Instance.Subscribe("OnScoreUpdate", OnScoreUpdate);
        EventManager.Instance.Subscribe("OnGameFinished", OnGameFinished);
    }

    protected void Update()
    {
        base.Update();
        
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player bullet
        if (other.gameObject.layer == 8)
            DestroyAsteroid();
    }

    private void DestroyAsteroid(bool hasScore = true)
    {
        if(hasScore)
            EventManager.Instance.Trigger("OnScoreUpdate", points);
        pool.ReturnToPool(this);
    }

    private void OnScoreUpdate(params object[] parameters)
    {
    }

    private void OnGameFinished(params object[] parameters)
    {
        DestroyAsteroid(false);
    }
}
