using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, ISpawner
{
    public float TimeToSpawn = 30;
    public int AsteroidCount = 8;
    public float AsteroidSpeed = 2f;

    private float _currentTime;
    private Transform _playerPos;
    private Pool<Asteroid> _asteroidPool;

    private void Start()
    {
        EventManager.Instance.Subscribe("OnGameFinished", OnGameFinished);

        AsteroidBuilder builder = new AsteroidBuilder();
        builder.SetSpeed(AsteroidSpeed);
        
        _asteroidPool = new Pool<Asteroid>(builder.Build, Asteroid.TurnOn, Asteroid.TurnOff, AsteroidCount);
        
        _currentTime = TimeToSpawn;
        _playerPos = GameObject.FindObjectOfType<PlayerModel>().GetComponent<Transform>();
        
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
        for (int i = 0; i < AsteroidCount; i++)
        {
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(-1f, 1f),Random.Range(-1f, 1f),10));
        
            var asteroid = _asteroidPool.Get();
            asteroid.pool = _asteroidPool;
            asteroid.transform.position = v3Pos;

            Vector3 diff = _playerPos.position - asteroid.transform.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            asteroid.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);    
        }

        _currentTime = TimeToSpawn;
    }

    private void OnGameFinished(params object[] parameters)
    {
        StopAsteroids();
    }

    public void StopAsteroids()
    {
        AsteroidCount = 0;
    }
}
