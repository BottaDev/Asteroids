using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Asteroids")]
    public float asteroidTimeToSpawn = 30;
    public int asteroidCount = 8;
    public float asteroidSpeed = 2f;
    [Header("Satelite Enemy")]
    public float sateliteSpeed = 1.5f;
    public float sateliteTimeToSpawn = 30f;
    public int sateliteCount = 2;
    [Header("Blaster Enemy")]
    public float blasterSpeed = 1.5f;
    public float blasterTimeToSpawn = 30f;
    public int blasterCount = 2;
    [Header("Elite Enemy")]
    public float eliteTimeToSpawn = 90f;
    public int eliteCount = 1;

    private float _currentAsteroidTime;
    private float _currentSateliteTime;
    private float _currentEliteTime;
    private Transform _playerPos;
    private Pool<Asteroid> _asteroidPool;
    private Pool<SateliteEnemy> _satelitePool;
    private Pool<ShipEnemy> _shipPool;
    private Pool<BlasterEnemy> _blasterPool;
    private Pool<EliteEnemy> _elitePool;

    private void Awake()
    {
        EventManager.Instance.Subscribe("OnGameFinished", OnGameFinished);

        AsteroidBuilder asteroidBuilder = new AsteroidBuilder();
        asteroidBuilder.SetSpeed(asteroidSpeed);

        SateliteBuilder sateliteBuilder = new SateliteBuilder();
        sateliteBuilder.SetSpeed(sateliteSpeed);

        BlasterBuilder blasterBuilder = new BlasterBuilder();
        blasterBuilder.SetSpeed(blasterSpeed);

        ShipBuilder shipBuilder = new ShipBuilder();
        EliteBuilder elitebuilder = new EliteBuilder();

        _asteroidPool = new Pool<Asteroid>(asteroidBuilder.Build, Asteroid.TurnOn, Asteroid.TurnOff, asteroidCount);
        _satelitePool = new Pool<SateliteEnemy>(sateliteBuilder.Build, SateliteEnemy.TurnOn, SateliteEnemy.TurnOff, sateliteCount);
        _blasterPool  = new Pool<BlasterEnemy>(blasterBuilder.Build, BlasterEnemy.TurnOn, BlasterEnemy.TurnOff, sateliteCount);
        _shipPool     = new Pool<ShipEnemy>(shipBuilder.Build, ShipEnemy.TurnOn, ShipEnemy.TurnOff, sateliteCount);
        _elitePool    = new Pool<EliteEnemy>(elitebuilder.Build, EliteEnemy.TurnOn, EliteEnemy.TurnOff, eliteCount);
    }

    private void Start()
    {
        _currentEliteTime = eliteTimeToSpawn;
        _playerPos = FindObjectOfType<PlayerModel>().GetComponent<Transform>();
        
        if(!SavestateManager.loaded)SpawnObject();
    }

    private void Update()
    {
        SpawnObject();
    }

    private void SpawnObject()
    {
        if (_currentAsteroidTime <= 0)
            Spawn(SpawnType.Asteroid);
        else
            _currentAsteroidTime -= Time.deltaTime;
        
        if (_currentSateliteTime <= 0)
            Spawn(SpawnType.SateliteEnemy);
        else
            _currentSateliteTime -= Time.deltaTime;
        
        if (_currentEliteTime <= 0)
            Spawn(SpawnType.EliteENemy);
        else
            _currentEliteTime -= Time.deltaTime;
    }

    public void Spawn(SpawnType obj)
    {
        switch (obj)
        {
            case SpawnType.Asteroid:
                for (int i = 0; i < asteroidCount; i++)
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
                
                _currentAsteroidTime = asteroidTimeToSpawn;
            break;
            
            case SpawnType.SateliteEnemy:
                for (int i = 0; i < sateliteCount; i++)
                {
                    Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(-1f, 1f),Random.Range(-1f, 1f),10));
                    
                    int random = Random.Range(0,3);

                    switch (random)
                    {
                        case 0:
                            var blaster = _blasterPool.Get();
                            blaster.pool = _blasterPool;
                            blaster.transform.position = v3Pos;
                            break;


                        case 1:
                            var ship = _shipPool.Get();
                            ship.pool = _shipPool;
                            ship.transform.position = v3Pos;
                            break;

                        case 2:
                            var satelite = _satelitePool.Get();
                            satelite.pool = _satelitePool;
                            satelite.transform.position = v3Pos;
                            break;

                    }
                }
                
                _currentSateliteTime = sateliteTimeToSpawn;
            break;

            case SpawnType.EliteENemy:
                for (int i = 0; i < eliteCount; i++)
                {
                    Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(-1f, 1f),Random.Range(-1f, 1f),10));
                    
                    var eliteEnemy = _elitePool.Get();
                    eliteEnemy.pool = _elitePool;
                    eliteEnemy.transform.position = v3Pos;
                }
                
                _currentEliteTime = eliteTimeToSpawn;
            break;
        }
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
        asteroidCount = 0;
    }

    public enum SpawnType
    {
        Asteroid,
        SateliteEnemy,
        EliteENemy,
    }
}
