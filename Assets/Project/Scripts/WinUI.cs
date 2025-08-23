using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    Score _score;
    TextMeshProUGUI _winScoreText;

    private void Start()
    {
        var scoreGO = GameObject.Find("Score(Clone)");
        if (scoreGO != null )
        {
            scoreGO.SetActive(false);
            _score = scoreGO.GetComponent<Score>();
        }
        _winScoreText = GameObject.Find("WinScore")?.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            Time.timeScale = 0f;
        }
        if (_score && _winScoreText)
        {
            _winScoreText.text = $"Player score : {_score.GetScore()}";
        }
    }

    // Start is called before the first frame update
    public void ReplayGame()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1.0f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1.0f;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
