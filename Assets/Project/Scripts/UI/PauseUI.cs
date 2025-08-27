using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    void OnEnable()
    {
        Time.timeScale = 0f;
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1f;
    }
}
