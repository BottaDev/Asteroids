using System.Collections.Generic;
using UnityEngine;
using KennethDevelops.Serialization;

public class PlayerModel : Entity
{
    public float Speed;
    public float RotationSpeed;
    public int lifes = 3;
    public float MaxSpeed;
    
    public List<IWeapon> weapons = new List<IWeapon>();

    private void Awake()
    {
        InitializeWeaponList();
    }

    private void Start()
    {
        EventManager.Instance.Subscribe("OnPlayerDamaged", OnPlayerDamaged);
        EventManager.Instance.Subscribe("OnSave", SaveData);
        EventManager.Instance.Subscribe("OnLoad", LoadData);

        LoadData();
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

    private void SaveData(params object[] parameters)
    {
        print("Player Data Saved");
        SavestateManager.Instance.saveState.playerData = new PlayerData(this);
    }

    private void LoadData(params object[] parameters)
    {
        PlayerData playerData = SavestateManager.Instance.saveState.playerData;

        transform.position = new Vector3(playerData.x, playerData.y, playerData.z);
        lifes = playerData.lives;
    }
}

[System.Serializable]
public class PlayerData
{
    public int lives;
    public float x;
    public float y;
    public float z;

    public PlayerData(PlayerModel player)
    {
        x = player.transform.position.x;
        y = player.transform.position.y;
        z = player.transform.position.z;

        lives = player.lifes;
    }
}