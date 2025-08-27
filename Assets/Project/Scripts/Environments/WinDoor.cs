using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDoor : MonoBehaviour
{
    [SerializeField] GameObject _winUI;
    [SerializeField] AudioClip  _winAudio;
    [SerializeField] AudioSource _audioSource;
    public event Action OnPlayerWin;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            //_winUI.SetActive(true);
            OnPlayerWin?.Invoke();
            _audioSource.PlayOneShot(_winAudio);
        }
    }
}
