using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : Entity
{
    public float Speed;
    public float RotationSpeed;
    public int lifes = 3;
    public float MaxSpeed;
    
    public List<IWeapon> weapons = new List<IWeapon>();
    [HideInInspector]
    public int currentWeaponIndex = 0;
    public float bulletSpeed = 10f;

    public void NextWeapon()
    {
        currentWeaponIndex++;
        if (currentWeaponIndex >= weapons.Count)
        {
            currentWeaponIndex = 0;
        }
    }

    private void Start()
    {
        EventManager.Instance.Subscribe("OnPlayerDead", OnPlayerDead);
        EventManager.Instance.Subscribe("OnPlayerDamaged", OnPlayerDamaged);
        EventManager.Instance.Subscribe("OnGameFinished", OnGameFinished);
    }

    protected void Update()
    {
        base.Update();

        if (lifes == 0)
        {
            gameObject.SetActive(false);
            EventManager.Instance.Trigger("OnGameFinished");
            EventManager.Instance.Trigger("OnPlayerDead");
        }
    }

    private void Awake()
    {
        InitializeWeaponList();
    }


    void InitializeWeaponList()
    {
        weapons.Add(new AutomaticWeapon());
        weapons.Add(new LaserWeapon());

        PlayerInput pInput = GetComponent<PlayerInput>();
        foreach (IWeapon item in weapons)
        {
            item.GetPlayerInput(pInput);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Asteroid
        if (other.gameObject.layer == 9)
        {
            lifes--;
            EventManager.Instance.Trigger("OnPlayerDamaged", lifes);
        }
    }

    private void OnPlayerDead(params object[] parameters)
    {
    }

    private void OnPlayerDamaged(params object[] parameters)
    {
    }

    private void OnGameFinished(params object[] parameters)
    {
    }
}