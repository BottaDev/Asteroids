using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Pool<Bullet> bulletPool;
    [HideInInspector]
    public float currentFireRate;
    [HideInInspector]
    public int currentWeaponIndex = 0;

    private Rigidbody2D _rb;
    private PlayerModel _playerModel;
    private float _auxAxisX;
    private float _auxAxisY;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _playerModel = GetComponent<PlayerModel>();
        
        currentFireRate = 0;
    }

    private void Start()
    {
        BulletBuilder builder = new BulletBuilder();
        builder.SetSpeed(_playerModel.bulletSpeed);
        
        bulletPool = new Pool<Bullet>(builder.Build, Bullet.TurnOn, Bullet.TurnOff, 5);

        currentFireRate = 0;
    }

    private void FixedUpdate()
    {
        _auxAxisX = Input.GetAxis(_playerModel.inputAxisX);
        _auxAxisY = Input.GetAxis(_playerModel.inputAxisY);

        Move();
    }

    private void Update()
    {
        if (_auxAxisX != 0)
            transform.Rotate(Vector3.forward * _playerModel.rotationSpeed * Time.deltaTime * -_auxAxisX);
            
        if (Input.GetKey(KeyCode.Space) && currentFireRate <= 0)
            _playerModel.weapons[currentWeaponIndex].Shoot();
        else if (Input.GetKeyDown(KeyCode.E))
            NextWeapon();
        else
            currentFireRate -= Time.deltaTime;
    }
    
    public void NextWeapon()
    {
        currentWeaponIndex++;
        if (currentWeaponIndex >= _playerModel.weapons.Count)
        {
            currentWeaponIndex = 0;
        }
    }

    private void Move()
    {
        if (_auxAxisY > 0)
        {
            StopCoroutine(Decelerate());

            _rb.AddForce(transform.up * _playerModel.speed );
            _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -_playerModel.maxSpeed, _playerModel.maxSpeed), 
                                        Mathf.Clamp(_rb.velocity.y, -_playerModel.maxSpeed, _playerModel.maxSpeed));
        }
        else
        {
            StartCoroutine(Decelerate());
        }
    }

    private IEnumerator Decelerate()
    {
        float t = 0;
        Vector3 fromVelocity = _rb.velocity;

        while (t < _playerModel.decelerationTime)
        {
            _rb.velocity = Vector3.Lerp(fromVelocity, Vector3.zero, t);
            t += Time.deltaTime / _playerModel.decelerationTime;

            yield return null;
        }
    }
}
