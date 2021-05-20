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
        EventManager.Instance.Subscribe("OnScoreUpdate", OnScoreUpdate);
        EventManager.Instance.Subscribe("OnPlayerDead", OnPlayerDead);
    }

    private void OnScoreUpdate(params object[] parameters)
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
        EventManager.Instance.Unsubscribe("OnScoreUpdate", OnScoreUpdate);
    }
}
