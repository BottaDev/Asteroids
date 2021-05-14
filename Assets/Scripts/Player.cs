using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : Entity
{
    public float Speed;
    public float RotationSpeed;
    
    public List<IWeapon> weapons = new List<IWeapon>();
    public int currentWeaponIndex = 0;
    
    [Range(min: 0, max: 1)]
    public float FireRate;
    
    public void NextWeapon()
    {
        currentWeaponIndex++;
        if (currentWeaponIndex >= weapons.Count)
        {
            currentWeaponIndex = 0;
        }
    }

    
    protected void Update()
    {
        base.Update();
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
            gameObject.SetActive(false);
    }
}
