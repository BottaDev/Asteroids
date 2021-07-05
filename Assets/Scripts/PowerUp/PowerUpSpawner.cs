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
        Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(-1f, 1f),Random.Range(-1f, 1f),10));
        
        var powerUp = _powerUpPool.Get();
        powerUp.pool = _powerUpPool;
        powerUp.transform.position = v3Pos;

        _currentTime = timeToSpawn;
    }
}
