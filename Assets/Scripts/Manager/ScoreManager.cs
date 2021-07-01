using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    private int _score = 0;
    public TMPro.TextMeshProUGUI scorePoints;
    public TMPro.TextMeshProUGUI finalPoints;

    private void Start()
    {
        EventManager.Instance.Subscribe("OnAsteroidDestroyed", OnAsteroidDestroyed);
        EventManager.Instance.Subscribe("OnPlayerDead", OnPlayerDead);
        EventManager.Instance.Subscribe("OnSave", SaveScore);
        EventManager.Instance.Subscribe("OnLoad", LoadScore);
    }

    private void OnAsteroidDestroyed(params object[] parameters)
    {
        var scoreRecived = (int)parameters[0];

        if (_score == 0)
            _score = scoreRecived;
        else
            _score += scoreRecived;
    }

    private void Update()
    {
        scorePoints.text = _score.ToString();
        finalPoints.text = _score.ToString();
    }

    private void OnPlayerDead(params object[] parameters)
    {
        EventManager.Instance.Unsubscribe("OnAsteroidDestroyeds", OnAsteroidDestroyed);
    }

    private void SaveScore(params object[] parameters)
    {
        print("Score Saved");
        GetComponent<SavestateManager>().saveState.score = _score;
    }
    private void LoadScore(params object[] parameters)
    {
        _score = GetComponent<SavestateManager>().saveState.score;
    }
}
