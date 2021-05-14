using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void RetryLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentLevelIndex);
    }

    public void ChangeLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
