using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TextMeshProUGUI _scoreText;
    int _score = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore(int score = 0)
    {
        _score += score;
        _scoreText.text = $"Score : {_score}";
    }

    public int GetScore()
    {
        return _score;
    }
}
