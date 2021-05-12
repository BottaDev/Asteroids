using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInput : MonoBehaviour
{
    [Header("Movement")]
    public string inputAxisX;
    public string inputAxisY;
    public Transform spawnPoint;
    
    public float currentFireRate;

    public float decelerationTime = 1.0f;
    
    private Rigidbody2D _rb;
    private Player _player;
    private Pool<Bullet> _bulletPool;
    private float _currentFireRate;
    private float _auxAxisX;
    private float _auxAxisY;
    private float _maxVelocity = 3;
    public bool _isStopping;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        _player = GetComponent<Player>();
        
        BulletFactory factory = new BulletFactory();
        bulletPool = new Pool<Bullet>(factory.Create, Bullet.TurnOn, Bullet.TurnOff, 5);
        
        currentFireRate = 0;
    }

    private void FixedUpdate()
    {
        _auxAxisX = Input.GetAxis(inputAxisX);
        _auxAxisY = Input.GetAxis(inputAxisY);

        if (_auxAxisY < 0)
        {
            _isStopping = true;
            StartCoroutine(Decelerate());
        }

        Move();
    }

    private void Update()
    {
    
        if (_auxAxisX != 0)
            transform.Rotate(Vector3.forward * _player.RotationSpeed * Time.deltaTime * -_auxAxisX);
            
            
        if (Input.GetKeyDown(KeyCode.Space) && currentFireRate <= 0)
            _player.weapons[_player.currentWeaponIndex].Shoot();
            
        else if (Input.GetKeyDown(KeyCode.E))
            _player.NextWeapon();

        else
            currentFireRate -= Time.deltaTime;
    }

}

    /*
    private void Move()
    {
        if (_isStopping && _auxAxisY < 0)
        {
            StopCoroutine(Decelerate());
            _isStopping = false;
        }
        
        if (_auxAxisY > 0)
            _rb.AddForce(transform.up * _auxAxisY);
    }
    
    private IEnumerator Decelerate()
    {
        float t = 0;
        Vector3 fromVelocity = _rb.velocity;
        
        while(t < decelerationTime)
        {
            _rb.velocity = Vector3.Lerp(fromVelocity, Vector3.zero, t);
            t += Time.deltaTime / decelerationTime;
            
            yield return null;
        }

        _isStopping = false;
    } 

    private void Shoot()
    {
        var bullet = _bulletPool.Get();

        bullet.pool = _bulletPool;
        bullet.transform.position = spawnPoint.position;
        bullet.transform.forward = spawnPoint.forward;

        _currentFireRate = _player.FireRate;
    }
    */
