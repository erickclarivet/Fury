using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenuUI : MonoBehaviour
{
    [SerializeField] GameObject _mainMenuUI;

    // Start is called before the first frame update
    public void BackToMenu()
    {
        gameObject.SetActive(false);
        _mainMenuUI.SetActive(true);
    }

    public void RunWithKeyboard()
    {
        PlayerPrefs.SetString("PlayerInput", "MoveKeyboard");
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }

    public void RunWithGamePad()
    {
        PlayerPrefs.SetString("PlayerInput", "MoveGamepad");
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }

}
