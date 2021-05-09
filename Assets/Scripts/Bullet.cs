﻿using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public float TimeToDestroy;
    public Pool<Bullet> pool;
    
    private float _spawnTime;

    private void OnEnable()
    {
        _spawnTime = Time.time;
    }

    private void Update()
    {
        transform.position += transform.up * (Speed * Time.deltaTime);
        
        if (_spawnTime + TimeToDestroy <= Time.time)
            DestroyBullet();
    }

    private void DestroyBullet()
    {
        pool.ReturnToPool(this);
    }
    
    public static void TurnOn(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    
    public static void TurnOff(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
}
