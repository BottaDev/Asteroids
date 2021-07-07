using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KennethDevelops.Serialization;

public class SavestateManager : MonoBehaviour
{
    public static SavestateManager Instance;

    public static bool loaded;
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

        if (loaded)
        {
            LoadData();
        }

        EventManager.Instance.Subscribe("OnPlayerDeath", ResetLoaded);
    }

    public void LoadGame()
    {
        loaded = true;
    }

    public void SaveGame()
    {
        saveState.asteroidList.Clear();
        EventManager.Instance.Trigger("OnSave");
        SaveData();
    }

    public void ResetLoaded() //this one is for the UI button call
    {
        loaded = false;
    }

    private void ResetLoaded(params object[] empty) //this one is for the delegate call
    {
        loaded = false;
    }

    private void SaveData()
    {
        saveState.SaveBinary(Application.dataPath + "/Resources/SaveGame/SaveState.dat");
    }

    public void LoadData()
    {
        saveState = BinarySerializer.LoadBinary<SaveState>(Application.dataPath + "/Resources/SaveGame/SaveState.dat");
        GetComponent<Spawner>()?.SpawnLoadedAsteroids(saveState.asteroidList);
        EventManager.Instance.Trigger("OnLoad", saveState.playerData.lives);
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