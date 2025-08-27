using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject _commandsMenuUI;

    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        gameObject.SetActive(false);
        _commandsMenuUI.SetActive(true);
    }
}
