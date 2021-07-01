using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpSpawner : MonoBehaviour, ISpawner
{
    public float TimeToSpawn = 30f;
    public int PowerUpCount = 1;
    
    private float _currentTime;
    private Pool<PowerUp> _powerUpPool;

    private void Awake()
    {
        EventManager.Instance.Subscribe("OnGameFinished", OnGameFinished);
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
        for (int i = 0; i < PowerUpCount; i++)
        {
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(-1f, 1f),Random.Range(-1f, 1f),10));
        
            var powerUp = _powerUpPool.Get();
            powerUp.pool = _powerUpPool;
            powerUp.transform.position = v3Pos;

            _currentTime = TimeToSpawn;   
        }
    }
    
    private void OnGameFinished(params object[] parameters)
    {
        PowerUpCount = 0;
    }
}
