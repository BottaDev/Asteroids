﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

//MyA1-P3
public class Bomb : MonoBehaviour
{
    public GameObject effect;
    public float radius = 2f;
    public float chainTime = 0.5f;
    public Pool<Bomb> pool;
    
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Explode(PlayerController controller)
    {
        GameObject obj =  Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(obj, chainTime);
        
        List<Asteroid> asteroids = GameObject.FindObjectsOfType<Asteroid>().Where(x => x.enabled = true).ToList();
        foreach (Asteroid ast in asteroids)
        {
            float distance = Vector2.Distance(transform.position, ast.transform.position);
            if (distance <= radius)
                ast.HitByBomb();
        }
        
        StartCoroutine(ExplodeNextBomb(controller));
    }

    private IEnumerator ExplodeNextBomb(PlayerController controller)
    {
        _renderer.enabled = false;
        
        // Gets index of current bomb in list
        int index = controller.activeBombs.FindIndex(x => x == this);
        if (index == controller.activeBombs.Count - 1)
        {
            controller.activeBombs.Clear();
            controller.exploding = false;
            
            DestroyBomb();
            yield return null;   
        }

        yield return new WaitForSeconds(chainTime);
        controller.activeBombs[index + 1].Explode(controller);
        
        DestroyBomb();
    }

    private void DestroyBomb()
    {
        pool.ReturnToPool(this);
    }

    private void OnEnable()
    {
        _renderer.enabled = true;
    }

    public static void TurnOn(Bomb bomb)
    {
        bomb.gameObject.SetActive(true);
    }
    
    public static void TurnOff(Bomb bomb)
    {
        bomb.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
