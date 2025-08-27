using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _winScoreText;

    void OnEnable()
    {
        Time.timeScale = 0f;
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void SetScore(int score)
    {
        _winScoreText.text = $"Score : {score}";
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
