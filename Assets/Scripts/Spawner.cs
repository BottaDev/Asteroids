using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, ISpawner
{
    public float TimeToSpawn;

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
        _currentTime = TimeToSpawn;
        _playerPos = GameObject.FindObjectOfType<Player>().GetComponent<Transform>();
        
        SpawnObject();
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
        
        var asteroid = _asteroidPool.Get();
        asteroid.pool = _asteroidPool;
        asteroid.transform.position = v3Pos;

        Vector3 diff = _playerPos.position - asteroid.transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        asteroid.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        
        _currentTime = TimeToSpawn;
    }
}
