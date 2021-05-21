using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject ingameScene;
    public GameObject loseScene;
    public GameObject[] lifeImages;
    int _lifes = 3;

    private void Start()
    {
        ingameScene.SetActive(true);
        loseScene.SetActive(false);

        EventManager.Instance.Subscribe("OnPlayerDamaged", OnPlayerDamaged);
        EventManager.Instance.Subscribe("OnPlayerDead", OnPlayerDead);
    }

    private void OnPlayerDamaged(params object[] parameters)
    {
        var lifeRecived = (int)parameters[0];
        _lifes = lifeRecived;
    }

    private void Update()
    {
        if (_lifes != 0)
            LifeImages();
        else
            DeadUI();
    }

    private void LifeImages()
    {
        switch (_lifes)
        {
            case 1:
                lifeImages[1].SetActive(false);
                break;

            case 2:
                lifeImages[2].SetActive(false);
                break;

            case 3:
                lifeImages[0].SetActive(true);
                lifeImages[1].SetActive(true);
                lifeImages[2].SetActive(true);
                break;
        }
    }

    private void OnPlayerDead(params object[] parameters)
    {
    }

    private void DeadUI()
    {
        ingameScene.SetActive(false);
        loseScene.SetActive(true);
    }
}
