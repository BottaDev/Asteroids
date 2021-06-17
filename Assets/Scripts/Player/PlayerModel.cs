using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : Entity
{
    public float Speed;
    public float RotationSpeed;
    public int lifes = 3;
    public float MaxSpeed;
    
    public List<IWeapon> weapons = new List<IWeapon>();

    private void Start()
    {
        EventManager.Instance.Subscribe("OnPlayerDead", OnPlayerDead);
        EventManager.Instance.Subscribe("OnPlayerDamaged", OnPlayerDamaged);
        EventManager.Instance.Subscribe("OnGameFinished", OnGameFinished);
    }

    private void Awake()
    {
        InitializeWeaponList();
    }


    void InitializeWeaponList()
    {
        weapons.Add(new AutomaticWeapon());
        weapons.Add(new LaserWeapon());

        PlayerController pController = GetComponent<PlayerController>();
        foreach (IWeapon item in weapons)
        {
            item.GetPlayerInput(pController);
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
        if (lifes == 0)
        {
            gameObject.SetActive(false);
            EventManager.Instance.Unsubscribe("OnPlayerDamaged", OnPlayerDamaged);
            EventManager.Instance.Trigger("OnGameFinished");
            EventManager.Instance.Trigger("OnPlayerDead");
        }
    }

    private void OnGameFinished(params object[] parameters)
    {
    }
}