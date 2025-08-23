using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDoor : MonoBehaviour
{
    [SerializeField] GameObject _winUI;
    [SerializeField] AudioClip  _winAudio;
    [SerializeField] AudioSource _audioSource;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            _winUI.SetActive(true);
            _audioSource.PlayOneShot(_winAudio);
        }
    }
}
