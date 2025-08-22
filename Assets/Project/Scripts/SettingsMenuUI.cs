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
}
