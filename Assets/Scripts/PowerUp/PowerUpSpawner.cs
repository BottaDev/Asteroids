using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpSpawner : MonoBehaviour, ISpawner
{
    public float timeToSpawn = 30f;
    public int powerUpCount = 1;
    public float timeToDestroy = 15f;

    private float _currentTime;
    private Pool<PowerUp> _powerUpPool;
    
    private void Start()
    {
        PowerUpBuilder builder = new PowerUpBuilder();
        builder.SetTime(timeToDestroy);
        _powerUpPool = new Pool<PowerUp>(builder.Build, PowerUp.TurnOn, PowerUp.TurnOff, powerUpCount);
        
        _currentTime = timeToSpawn;
    }

    private void Update()
    {
        CheckTime();
    }
    
    private void CheckTime()
    {
        if (_currentTime <= 0)
            SpawnObject();
        else
            _currentTime -= Time.deltaTime;
    }
    
    public void SpawnObject()
    {
        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0,Screen.width), Random.Range(0,Screen.height), Camera.main.farClipPlane/2));
        
        var powerUp = _powerUpPool.Get();
        powerUp.pool = _powerUpPool;
        powerUp.transform.position = screenPosition;

        _currentTime = timeToSpawn;
    }
}
