using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, ISpawner
{
    public float timeToSpawn;

    private float _currentTime;
    private Transform _playerPos;
    private Pool<Asteroid> _asteroidPool;

    private void Awake()
    {
        AsteroidFactory factory = new AsteroidFactory();

        _asteroidPool = new Pool<Asteroid>(factory.Create, Asteroid.TurnOn, Asteroid.TurnOff, 10);
    }

    private void Start()
    {
        _currentTime = timeToSpawn;
        _playerPos = GameObject.FindObjectOfType<Player>().GetComponent<Transform>();
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
        // Spawnear usando pool
    }
}
