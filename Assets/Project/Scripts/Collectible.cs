using System;
using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] int _points = 10;
    [SerializeField] AudioClip _collectSound;
    [SerializeField] AudioSource _collectSource;

    public event Action<int> OnAddPoints;
    public event Action<Collectible> OnDestroyCollectible;
    public int GetPoints()
    {
        return _points;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnAddPoints?.Invoke(_points);
            StartCoroutine(LaunchSound());
        }
    }

    public IEnumerator LaunchSound()
    {
        _collectSource.PlayOneShot(_collectSound);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        OnDestroyCollectible?.Invoke(GetComponent<Collectible>());
    }

}
