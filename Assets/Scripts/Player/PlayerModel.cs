using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : Entity
{
    [Header("Parameters")] 
    public float Speed;
    public float RotationSpeed;
    public float MaxSpeed;
    public int lifes = 3;
    public float decelerationTime = 1.0f;
    [Header("Feedback")]
    public float damageColorTime = 0.3f;
    [Header("Movement")] 
    public string inputAxisX;
    public string inputAxisY;
    [Header("Bullets")]
    public Transform spawnPoint;
    public float bulletSpeed = 10f;
    [Header("Weapons")]
    public List<IWeapon> weapons = new List<IWeapon>();

    private void Awake()
    {
        InitializeWeaponList();
    }

    private void Start()
    {
        EventManager.Instance.Subscribe("OnPlayerDamaged", OnPlayerDamaged);
    }

    void InitializeWeaponList()
    {
        weapons.Add(new AutomaticWeapon());
        weapons.Add(new LaserWeapon());
        
        foreach (IWeapon item in weapons)
        {
            item.GetPlayerInput(this, GetComponent<PlayerController>());
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
}