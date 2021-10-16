using System.Collections.Generic;
using UnityEngine;
using KennethDevelops.Serialization;

public class PlayerModel : Entity
{
    [Header("Properties")]
    public float Speed = 6f;
    public float RotationSpeed = 450f;
    public int lifes = 3;
    public float MaxSpeed = 4.5f;
    [Header("Player Movement")] 
    public string inputAxisX = "Horizontal";
    public string inputAxisY = "Vertical";
    public Transform spawnPoint;
    [HideInInspector]
    public float currentFireRate;
    public float decelerationTime = 1.0f;
    [Header("Bullets")]
    [HideInInspector]
    public int currentWeaponIndex = 0;
    public float bulletSpeed = 10f;
    [Header("Bombs")]
    public float chainTime = 0.5f;
    public float radius = 2f;
    [Header("Feedback")]
    public GameObject spriteFire;
    public float colorTime = 0.3f;
    
    public List<IWeapon> weapons = new List<IWeapon>();

    private void Awake()
    {
        InitializeWeaponList();
    }

    protected override void Start()
    {
        base.Start();
        
        EventManager.Instance.Subscribe("OnSave", OnSaveData);
        EventManager.Instance.Subscribe("OnLoad", OnLoadData);

        OnLoadData();
    }

    void InitializeWeaponList()
    {
        weapons.Add(new AutomaticWeapon());
        weapons.Add(new LaserWeapon());
        weapons.Add(new BombWeapon());

        PlayerController pController = GetComponent<PlayerController>();
        foreach (IWeapon item in weapons)
        {
            item.GetPlayerInput(this, pController);
        }
    }

    private void OnSaveData(params object[] parameters)
    {
        print("Player Data Saved");
        SavestateManager.Instance.saveState.playerData = new PlayerData(this);
    }

    private void OnLoadData(params object[] parameters)
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