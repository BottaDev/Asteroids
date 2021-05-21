﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInput : MonoBehaviour
{
    [Header("Movement")] public string inputAxisX;
    public string inputAxisY;
    public Transform spawnPoint;
    [HideInInspector]
    public Pool<Bullet> bulletPool;
    [HideInInspector]
    public float currentFireRate;
    public float decelerationTime = 5.0f;

    private Rigidbody2D _rb;
    private Player _player;
    private float _auxAxisX;
    private float _auxAxisY;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _player = GetComponent<Player>();
        
        currentFireRate = 0;
    }

    private void Start()
    {
        BulletBuilder builder = new BulletBuilder();
        builder.SetSpeed(_player.bulletSpeed);
        
        bulletPool = new Pool<Bullet>(builder.Build, Bullet.TurnOn, Bullet.TurnOff, 5);

        currentFireRate = 0;
    }

    private void FixedUpdate()
    {
        _auxAxisX = Input.GetAxis(inputAxisX);
        _auxAxisY = Input.GetAxis(inputAxisY);

        Move();
    }

    private void Update()
    {
        if (_auxAxisX != 0)
            transform.Rotate(Vector3.forward * _player.RotationSpeed * Time.deltaTime * -_auxAxisX);
            
        if (Input.GetKey(KeyCode.Space) && currentFireRate <= 0)
            _player.weapons[_player.currentWeaponIndex].Shoot();
        else if (Input.GetKeyDown(KeyCode.E))
            _player.NextWeapon();
        else
            currentFireRate -= Time.deltaTime;
    }

    private void Move()
    {
        if (_auxAxisY > 0)
        {
            StopCoroutine(Decelerate());

            _rb.AddForce(transform.up * _player.Speed );
            _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -_player.MaxSpeed, _player.MaxSpeed), 
                                        Mathf.Clamp(_rb.velocity.y, -_player.MaxSpeed, _player.MaxSpeed));
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

        while (t < decelerationTime)
        {
            _rb.velocity = Vector3.Lerp(fromVelocity, Vector3.zero, t);
            t += Time.deltaTime / decelerationTime;

            yield return null;
        }
    }
}
