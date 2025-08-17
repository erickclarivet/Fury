using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDoor : MonoBehaviour
{
    [SerializeField] private GameObject _winUI;
    [SerializeField] private AudioClip  _winAudio;
    [SerializeField] private AudioSource _audioSource;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            _winUI.SetActive(true);
            _audioSource.PlayOneShot(_winAudio);
        }
    }
}
