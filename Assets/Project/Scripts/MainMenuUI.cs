using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject _settingsMenuUI;

    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Settings()
    {
        gameObject.SetActive(false);
        _settingsMenuUI.SetActive(true);
    }
}
