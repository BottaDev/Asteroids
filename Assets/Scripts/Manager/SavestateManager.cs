using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KennethDevelops.Serialization;

public class SavestateManager : MonoBehaviour
{
    public static SavestateManager Instance;

    public bool loaded;
    public SaveState saveState;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogWarning("Duplicated detected found" + gameObject.name);
            Destroy(this);
        }
    }

    private void Start()
    {
        LoadData();
    }

    public void SaveData()
    {
        print("All Saved!");
        saveState.SaveBinary(Application.dataPath + "/Resources/SaveGame/SaveState.dat");
    }

    public void LoadData()
    {
        saveState = BinarySerializer.LoadBinary<SaveState>(Application.dataPath + "/Resources/SaveGame/SaveState.dat");
        GetComponent<Spawner>().SpawnLoadedAsteroids(saveState.asteroidList);
        EventManager.Instance.Trigger("OnLoad");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            saveState.asteroidList.Clear();
            EventManager.Instance.Trigger("OnSave");
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            LoadData();
        }
    }

}

[System.Serializable]
public class SaveState
{
    public float spawnerCooldown;
    public int score;
    public PlayerData playerData;
    public List<AsteroidData> asteroidList;
}