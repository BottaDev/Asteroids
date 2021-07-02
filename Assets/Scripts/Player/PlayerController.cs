﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")] 
    public string inputAxisX;
    public string inputAxisY;
    public Transform spawnPoint;
    [HideInInspector]
    public Pool<Bullet> bulletPool;
    [HideInInspector]
    public float currentFireRate;
    public float decelerationTime = 5.0f;
    [Header("Bullets")]
    [HideInInspector]
    public int currentWeaponIndex = 0;
    public float bulletSpeed = 10f;

    public GameObject spriteFire;
    
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
        builder.SetSpeed(bulletSpeed);
        
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
            transform.Rotate(Vector3.forward * _playerModel.RotationSpeed * Time.deltaTime * -_auxAxisX);
            
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

            _rb.AddForce(transform.up * _playerModel.Speed );
            _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -_playerModel.MaxSpeed, _playerModel.MaxSpeed), 
                                        Mathf.Clamp(_rb.velocity.y, -_playerModel.MaxSpeed, _playerModel.MaxSpeed));

            EventManager.Instance.Trigger("OnPlayerMove", spriteFire);
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
