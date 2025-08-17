using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private GameObject _collectible;
    [SerializeField] private int _points = 10;
    [SerializeField] private AudioClip _collectSound;
    [SerializeField] private AudioSource _collectSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPoints()
    {
        return _points;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().AddPoints(_points);
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

}
