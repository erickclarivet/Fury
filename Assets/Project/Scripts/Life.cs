using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    int _life = 4;
    List<Transform> _hearts;
    [SerializeField] GameObject _heartFullPrefab;
    [SerializeField] Sprite _emptyHeart;

    public int Instantiate(int life)
    {
        Vector3 position = new Vector3(0, 0, 0);
        _hearts = new List<Transform>();
        for (int i = 0; i < _life; i++)
        {
            GameObject newhHeart = Instantiate(_heartFullPrefab);
            newhHeart.transform.SetParent(this.transform);
            newhHeart.transform.localScale = new Vector3(100, 100, 0);
            newhHeart.transform.localPosition = new Vector3(position.x + i * 60, position.y, position.z);
            _hearts.Add(newhHeart.transform);
        }
        return _life;
    }

    public void LooseHeart(int hit)
    {
        if (hit == -1)
            hit = _life;
        for (int i = 0; i < hit && _life >= 0; i++)
        {
            StartCoroutine(BlinkRed());
            _life--;
        }
    }

    public IEnumerator BlinkRed()
    {
        var spriteRenderer = _hearts[_life - 1].GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = _emptyHeart;
        spriteRenderer.color = Color.white;
    }

    public bool IsAlive()
    {
        return _life == 0;
    }
}
