using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, ISpawner
{
    public float TimeToSpawn = 30;
    public int EntityCount = 8;
    public float AsteroidSpeed = 2f;
    public float SateliteSpeed = 1.5f;

    private float _currentTime;
    private Transform _playerPos;
    private Pool<Asteroid> _asteroidPool;
    private Pool<SateliteEnemy> _satelitePool;
    private Pool<ShipEnemy> _shipPool;

    private void Awake()
    {
        EventManager.Instance.Subscribe("OnGameFinished", OnGameFinished);

        AsteroidBuilder asteroidBuilder = new AsteroidBuilder();
        asteroidBuilder.SetSpeed(AsteroidSpeed);

        SateliteBuilder sateliteBuilder = new SateliteBuilder();
        sateliteBuilder.SetSpeed(SateliteSpeed);

        ShipBuilder shipBuilder = new ShipBuilder();


        _asteroidPool = new Pool<Asteroid>(asteroidBuilder.Build, Asteroid.TurnOn, Asteroid.TurnOff, EntityCount);
        _satelitePool = new Pool<SateliteEnemy>(sateliteBuilder.Build, SateliteEnemy.TurnOn, SateliteEnemy.TurnOff, EntityCount);
        _shipPool     = new Pool<ShipEnemy>(shipBuilder.Build, ShipEnemy.TurnOn, ShipEnemy.TurnOff, EntityCount);
    }

    private void Start()
    {   
        _currentTime = TimeToSpawn;
        _playerPos = GameObject.FindObjectOfType<PlayerModel>().GetComponent<Transform>();
        
        if(!SavestateManager.loaded)SpawnObject();
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
        for (int i = 0; i < EntityCount; i++)
        {
            Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(-1f, 1f),Random.Range(-1f, 1f),10));

            int random = Random.Range(0,3);

            switch (random)
            {
                case 0: //Case Asteroid

                    var asteroid = _asteroidPool.Get();
                    asteroid.pool = _asteroidPool;
                    asteroid.transform.position = v3Pos;

                    Vector3 diff = _playerPos.position - asteroid.transform.position;
                    diff.Normalize();
                    float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    asteroid.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                    break;

                case 1: //Case Satelite

                    var satelite = _satelitePool.Get();
                    satelite.pool = _satelitePool;
                    satelite.transform.position = v3Pos;
                    break;

                case 2: //Case Ship

                    var ship = _shipPool.Get();
                    ship.pool = _shipPool;
                    ship.transform.position = v3Pos;
                    break;
            }

        }

        _currentTime = TimeToSpawn;
    }

    public void SpawnLoadedAsteroids(List<AsteroidData> asteroidData)
    {
        foreach (var item in asteroidData)
        {
            print(_asteroidPool);
            var asteroid  = _asteroidPool.Get();
            asteroid.pool = _asteroidPool;
            asteroid.transform.position = new Vector3(item.x, item.y, item.z);
            asteroid.transform.rotation = Quaternion.Euler(0f, 0f, item.zRotation);
        }
    }

    private void OnGameFinished(params object[] parameters)
    {
        StopAsteroids();
    }

    public void StopAsteroids()
    {
        EntityCount = 0;
    }
}
