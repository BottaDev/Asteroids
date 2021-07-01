using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject ingameScene;
    public GameObject loseScene;
    public GameObject[] lifeImages;
    
    private int _lifes = 3;

    private void Awake()
    {
        ingameScene.SetActive(true);
        loseScene.SetActive(false);

        EventManager.Instance.Subscribe("OnPlayerDamaged", OnPlayerDamaged);
        EventManager.Instance.Subscribe("OnPlayerHealed", OnPlayerHealed);
        EventManager.Instance.Subscribe("OnLoad", OnPlayerDamaged);
    }

    private void OnPlayerHealed(params object[] parameters)
    {
        var lifeRecived = (int)parameters[0];
        _lifes = lifeRecived;
        
        LifeImages();
    }
    
    private void OnPlayerDamaged(params object[] parameters)
    {
        var lifeRecived = (int)parameters[0];
        print(lifeRecived);
        _lifes = lifeRecived;
        
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
                lifeImages[0].SetActive(false);
                lifeImages[1].SetActive(false);
                lifeImages[2].SetActive(true);

                break;

            case 2:
                lifeImages[0].SetActive(false);
                lifeImages[1].SetActive(true);
                lifeImages[2].SetActive(true);
                break;

            case 3:
                lifeImages[0].SetActive(true);
                lifeImages[1].SetActive(true);
                lifeImages[2].SetActive(true);
                break;
        }
    }

    private void DeadUI()
    {
        ingameScene.SetActive(false);
        loseScene.SetActive(true);
    }
}
